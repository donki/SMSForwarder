namespace SMSForwarder
{
    /// <summary>
    /// Lo que las pantallas piden al dispositivo (dialogos, permisos, portapapeles, navegador,
    /// version, navegacion del Shell, hilo principal) pasa por aqui. En la app cada puerta es la de
    /// MAUI o la del dialogo moderno comun; las pruebas (constitucion General 8.6) ponen dobles para
    /// recorrer las pantallas sin dispositivo. Mismas firmas que SocShared.ModernDialog y
    /// Permissions, para que el cambio en las paginas sea solo el nombre.
    /// </summary>
    public static class AppPlatform
    {
        private static IClipboard? _clipboard;
        private static ILauncher? _launcher;
        private static IEmail? _email;
        private static IAppInfo? _appInfo;

        public static IClipboard Clipboard { get => _clipboard ??= Microsoft.Maui.ApplicationModel.DataTransfer.Clipboard.Default; set => _clipboard = value; }

        public static ILauncher Launcher { get => _launcher ??= Microsoft.Maui.ApplicationModel.Launcher.Default; set => _launcher = value; }

        public static IEmail Email { get => _email ??= Microsoft.Maui.ApplicationModel.Communication.Email.Default; set => _email = value; }

        public static IAppInfo AppInfo { get => _appInfo ??= Microsoft.Maui.ApplicationModel.AppInfo.Current; set => _appInfo = value; }

        public static Func<Page, string, string, string, string?, Task<bool>> AlertHandler { get; set; } =
            (page, title, message, accept, cancel) => SocShared.ModernDialog.AlertAsync(page, title, message, accept, cancel);

        public static Func<Page, string, string?, string, string, string?, string?, bool, Task<string?>> PromptHandler { get; set; } =
            (page, title, message, accept, cancel, initial, placeholder, password) =>
                SocShared.ModernDialog.PromptAsync(page, title, message, accept, cancel, initial, placeholder, password);

        public static Func<Page, string?, string, string[], Task<string?>> ActionSheetHandler { get; set; } =
            (page, title, cancel, options) => SocShared.ModernDialog.ActionSheetAsync(page, title, cancel, options);

        /// <summary>Navegacion por rutas del Shell (ruta, parametros).</summary>
        public static Func<string, IDictionary<string, object>?, Task> GoToHandler { get; set; } =
            (route, parameters) => parameters switch
            {
                null => Shell.Current.GoToAsync(route),
                // Los de un solo uso se pasan tal cual, sin codificarlos en la ruta.
                ShellNavigationQueryParameters once => Shell.Current.GoToAsync(route, once),
                _ => Shell.Current.GoToAsync(route, parameters)
            };

        public static Action<Action> MainThreadHandler { get; set; } = MainThread.BeginInvokeOnMainThread;

        public static Func<Permissions.BasePermission, Task<PermissionStatus>> CheckPermission { get; set; } = p => p.CheckStatusAsync();

        public static Func<Permissions.BasePermission, Task<PermissionStatus>> RequestPermission { get; set; } = p => p.RequestAsync();

        /// <summary>
        /// Oculta la app sin cerrarla (atras en inicio, Mobile 7). La pone MainActivity en Android;
        /// devuelve false donde no hay nada que ocultar.
        /// </summary>
        public static Func<bool> MoveTaskToBack { get; set; } = () => false;

        /// <summary>
        /// Selector del sistema con las apps de correo (para, asunto, titulo). Lo pone MainActivity
        /// en Android; false si no lo hay, y entonces se usa el correo de MAUI.
        /// </summary>
        public static Func<string, string, string, bool> StartEmailChooser { get; set; } = (_, _, _) => false;

        public static Task<bool> AlertAsync(Page page, string title, string message, string accept, string? cancel = null) =>
            AlertHandler(page, title, message, accept, cancel);

        public static Task<string?> PromptAsync(Page page, string title, string? message, string accept = "OK",
            string cancel = "Cancel", string? initialValue = null, string? placeholder = null, bool isPassword = false) =>
            PromptHandler(page, title, message, accept, cancel, initialValue, placeholder, isPassword);

        public static Task<string?> ActionSheetAsync(Page page, string? title, string cancel, params string[] options) =>
            ActionSheetHandler(page, title, cancel, options);

        public static Task GoToAsync(string route) => GoToHandler(route, null);

        public static Task GoToAsync(string route, IDictionary<string, object> parameters) => GoToHandler(route, parameters);

        public static void BeginInvokeOnMainThread(Action action) => MainThreadHandler(action);

        public static Task<PermissionStatus> CheckStatusAsync<T>() where T : Permissions.BasePermission, new() =>
            CheckPermission(new T());

        public static Task<PermissionStatus> RequestAsync<T>() where T : Permissions.BasePermission, new() =>
            RequestPermission(new T());
    }
}
