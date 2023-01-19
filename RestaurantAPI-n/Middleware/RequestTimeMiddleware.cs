using System.Diagnostics;

namespace RestaurantAPI_n.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private Stopwatch _stopWatch;
        private readonly ILogger<RequestTimeMiddleware> _logger;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _stopWatch = new Stopwatch();
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopWatch.Start();
            await next.Invoke(context);
            _stopWatch.Stop();

            var elapsedMs = _stopWatch.ElapsedMilliseconds;
            if(elapsedMs / 1000 < 4)
            {
                var message = $"Request [{context.Request.Method} at {context.Request.Path} took {elapsedMs} ms]";
                _logger.LogInformation(message);
            }
        }
    }
}
