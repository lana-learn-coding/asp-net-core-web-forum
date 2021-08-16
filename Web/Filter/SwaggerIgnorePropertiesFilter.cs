using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Web.Filter
{
    // Help swagger ignore props with attribute [JsonIgnore]
    // when generate doc for param or form param
    public class SwaggerIgnorePropertiesFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription == null || operation.Parameters == null)
                return;

            if (!context.ApiDescription.ParameterDescriptions.Any())
                return;

            var queries = context.ApiDescription.ParameterDescriptions.Where(p =>
                p.Source.Equals(BindingSource.Query) && p.CustomAttributes().Any(p => p is JsonIgnoreAttribute));
            foreach (var queryParam in queries)
            {
                operation.Parameters.Remove(operation.Parameters.Single(w => w.Name.Equals(queryParam.Name)));
            }
        }
    }
}