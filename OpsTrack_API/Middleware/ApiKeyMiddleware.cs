namespace OpsTrack_API.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string HeaderName = "X-Api-Key";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration config)
        {
            // Kun POST skal kræve nøgle
            if (context.Request.Method == HttpMethods.Post)
            {
                if (!context.Request.Headers.TryGetValue(HeaderName, out var extractedApiKey))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("API Key missing");
                    return;
                }

                var apiKey = config.GetValue<string>("ApiKey");
                if (!apiKey.Equals(extractedApiKey))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Unauthorized client");
                    return;
                }
            }

            await _next(context);
        }
    }

}
