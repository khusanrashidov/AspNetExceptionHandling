using Microsoft.AspNetCore.Http;
using School.API.Data.Responses;
using System;
using System.Net;
using System.Threading.Tasks;

namespace School.API.Exceptions
{
    public class CustomExceptionHandler
    {
        // inject
        private readonly RequestDelegate _next;

        public CustomExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            httpContext.Response.ContentType = "application/json";
            var errorMessageString = new ErrorResponseData()
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = e.Message,
                Path = httpContext.Request.Path
            }.ToString();
            return httpContext.Response.WriteAsync(errorMessageString);
        }
    }
}