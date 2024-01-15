using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Http.Extensions;
using System.Diagnostics;

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
            Stopwatch watch = Stopwatch.StartNew();

            //do something with the request
            string path = context.Request.Path;
            string method = context.Request.Method;
            string origin = context.Request.Headers["origin"].FirstOrDefault();
            string timeStamp = "["+DateTime.Now.ToString()+"]";
            await _next(context);
            ////do something with the response (after it sent)
             watch.Stop();
            long responseTime = watch.ElapsedMilliseconds;
            int statusCode = context.Response.StatusCode;

            if (statusCode >= 400)
            {
                Console.ForegroundColor= ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine($"{timeStamp} incoming request from {origin} {path} {method} {statusCode} - {responseTime}ms");
            Console.ResetColor();
        } // date time - path - method - status code - response time 
        //ex: [15/01/2024 18:28:35] incoming request from {origin} /api/users GET 200 - 10ms 


    }
}
