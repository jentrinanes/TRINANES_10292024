namespace FileProcessor.API
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _apiKey;

        public ApiKeyMiddleware(RequestDelegate next, string apiKey)
        {
            _next = next;
            _apiKey = apiKey;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("X-API-Key", out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key is missing");
                return;
            }

            var apiKey = extractedApiKey.FirstOrDefault();
            if (!apiKey.Equals(_apiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key is invalid");
                return;
            }

            await _next(context);
        }
    }
}
