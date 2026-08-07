namespace SMSForwarder.Platforms.Android
{
    /// <summary>
    /// Buzon de un solo hueco para la peticion de "redactar mensaje" que llega desde fuera
    /// (<see cref="ComposeSmsActivity"/>). La actividad de entrada es efimera y no puede navegar
    /// por Shell, asi que deja aqui destinatario y cuerpo y <c>MainActivity</c> los recoge cuando
    /// la UI de MAUI ya esta viva.
    /// </summary>
    public static class PendingCompose
    {
        private static string? _to;
        private static string? _body;
        private static bool _pending;

        public static void Set(string? to, string? body)
        {
            _to = to;
            _body = body;
            _pending = true;
        }

        /// <summary>Devuelve y consume la peticion pendiente. False si no habia ninguna.</summary>
        public static bool TryTake(out string? to, out string? body)
        {
            to = _to;
            body = _body;
            var had = _pending;
            _pending = false;
            _to = null;
            _body = null;
            return had;
        }
    }
}
