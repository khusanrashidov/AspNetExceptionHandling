using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using School.API.Data.Responses;
using System.Net;

namespace School.API.Exceptions
{
    public static class ExceptionMiddlewareExtensions
    {
        // Built-in exception handler.
        public static void ConfigureBuiltInExceptionHandler(this IApplicationBuilder app)
        {
            // action delegate
            app.UseExceptionHandler(appError =>
            {
                // request delegate handler
                // Request delegates are used to build the request pipeline. The request delegates handle each
                // HTTP request. Request delegates are configured using Run, Map, and Use extension methods.
                appError.Run(async context => // async lambda expression
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var contextRequest = context.Features.Get<IHttpRequestFeature>();

                    context.Response.ContentType = "application/json";

                    if (contextFeature != null)
                    {
                        var errorString = new ErrorResponseData()
                        {
                            StatusCode = (int)HttpStatusCode.InternalServerError,
                            Message = contextFeature.Error.Message,
                            Path = contextRequest.Path
                        }.ToString();
                        await context.Response.WriteAsync(errorString);
                    }
                });
            });
        }

        public static void ConfigureCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionHandler>();
        }
    }
}