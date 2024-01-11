using Amazon.Runtime.Internal;

namespace DotNetCardsServer.Middlewares
{
    public class ReqResLoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public ReqResLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //do something with the request
            string path = context.Request.Path;
            string method = context.Request.Method;
            string referer = context.Request.Headers["Referer"].ToString();
            var origin = context.Request.Headers["Origin"].ToString();

            Console.WriteLine($"we got new request from referer {referer} to {path} with method {method}");

            await _next(context);

            ////do something with the response (after it sent)
            string statusCode = context.Response.StatusCode.ToString();
            Console.WriteLine("We sent back response with status " + statusCode);

        }


    }
}
