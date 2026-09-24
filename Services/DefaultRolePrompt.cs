namespace SMSForwarder.Services
{
    /// <summary>
    /// Politica de Play para gestores por defecto: el dialogo de "app de SMS predeterminada" tiene
    /// que salir antes que cualquier permiso en tiempo de ejecucion. MainActivity lo pide al
    /// arrancar y avisa aqui al terminar; las paginas que piden permisos (el buzon se abre a la vez
    /// que la actividad) esperan a <see cref="Done"/> antes de pedir nada.
    /// </summary>
    public static class DefaultRolePrompt
    {
#if ANDROID
        private static readonly TaskCompletionSource _done =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
#else
        private static readonly TaskCompletionSource _done = Completed();

        private static TaskCompletionSource Completed()
        {
            var tcs = new TaskCompletionSource();
            tcs.SetResult();
            return tcs;
        }
#endif

        public static Task Done => _done.Task;

        public static void MarkDone() => _done.TrySetResult();
    }
}
