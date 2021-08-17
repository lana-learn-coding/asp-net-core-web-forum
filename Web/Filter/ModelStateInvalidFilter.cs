using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Filter
{
    // Customize 400 validation response to fit with vue
    public class ModelStateInvalidFilter : IActionFilter, IOrderedFilter
    {
        public int Order => -2000;

        public static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
        };

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Result == null && !context.ModelState.IsValid)
                context.Result = new JsonResult(new SerializableError(context.ModelState))
                {
                    SerializerSettings = SerializerOptions,
                    StatusCode = 400
                };
        }
    }
}