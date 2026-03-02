using LibraryManagement.Domain.ErrorModels;
using LibraryManagement.Domain.Exceptions;
using System.Net;

namespace LibraryManagement.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                //tu rame crashi moxdeba ak daichers
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Default to 500 Internal Server Error
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "Internal Server Error from the custom middleware.";

            // Switch based on the TYPE of exception
            switch (exception)
            {
                case NotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound; // 404
                    message = exception.Message;
                    break;

                case BadRequestException:
                    statusCode = (int)HttpStatusCode.BadRequest; // 400
                    message = exception.Message;
                    break;

                case BusinessRuleException:
                    statusCode = (int)HttpStatusCode.Conflict; // 409 (or 400)
                    message = exception.Message;
                    break;

            }

            context.Response.StatusCode = statusCode;

            var errorResponse = new ErrorDetails
            {
                StatusCode = statusCode,
                Message = message
            };

            await context.Response.WriteAsync(errorResponse.ToString());
        }
    }
}