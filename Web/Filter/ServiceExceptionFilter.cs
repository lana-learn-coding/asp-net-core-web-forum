using System.Collections.Generic;
using Core.Services.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Filter
{
    public class ServiceExceptionFilter : IActionFilter, IOrderedFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            switch (context.Exception)
            {
                case DataNotFoundException:
                    context.ExceptionHandled = true;
                    context.Result = new NotFoundResult();
                    return;

                case ForbiddenException exception:
                    context.ExceptionHandled = true;
                    context.Result = new JsonResult(new { exception.Message })
                    {
                        StatusCode = 403
                    };
                    return;

                case InvalidDataException exception:
                    context.ExceptionHandled = true;
                    context.Result = new JsonResult(exception.ModelErrors)
                    {
                        SerializerSettings = ModelStateInvalidFilter.SerializerOptions,
                        StatusCode = 400
                    };
                    return;

                case ConflictException exception:
                    context.ExceptionHandled = true;
                    context.Result = new JsonResult(new { exception.Message })
                    {
                        StatusCode = 409
                    };
                    return;

                case ServiceException exception:
                    context.ExceptionHandled = true;
                    var message = exception.InnerException?.Message ?? exception.Message;
                    context.Result = new JsonResult(new { Message = message })
                    {
                        StatusCode = 500
                    };
                    return;
            }
        }

        public int Order => -10;
    }
}