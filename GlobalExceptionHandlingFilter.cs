using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MyProject.Business.Models;
using System.Net;

namespace Scheduler.Api.GlobalFilters
{
    public class GlobalExceptionHandlingFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionHandlingFilter> _logger;

        public GlobalExceptionHandlingFilter(ILogger<GlobalExceptionHandlingFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError($"API: {context.Exception.Message}", context.Exception);

            //_logger.LogError(new EventId(context.Exception.HResult), context.Exception, $"API: {context.Exception.Message}");

            var errorDetails = new Notification
            {
                Statuscode = HttpStatusCode.InternalServerError,
                Success = false,
                Error = "An unexpected error ocurred"
            };
            
            context.ExceptionHandled = true;
            context.Result = new ObjectResult(errorDetails)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
