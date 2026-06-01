using BookstoreApplication.Services.Exceptions;
using System.Text.Json;

namespace BookstoreApplication.Controllers.Middleware
{
    internal sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        public ExceptionHandlingMiddleware() { }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = e switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
            var response = new
            {
                error = e.Message
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
