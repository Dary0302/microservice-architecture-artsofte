namespace CoreLib.Http
{
    public static class TraceIdProvider
    {
        private static readonly AsyncLocal<string?> traceId = new();

        public static string TraceId
        {
            get => traceId.Value ??= Guid.NewGuid().ToString();
            set => traceId.Value = value;
        }
    }
}