namespace Menza.WebApi.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string API_KEY_HEADER = "X-API-KEY";
        private readonly string _apiKey;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _apiKey = configuration["ApiKey"]; // store in appsettings.json or secrets
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var extractedApiKey))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("API Key missing");
                return;
            }

            if (!string.Equals(extractedApiKey, _apiKey))
            {
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }

            await _next(context);
        }
    }

}
