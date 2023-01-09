using Asp.Versioning;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DemoAuth
{
    /// <summary>
    /// Swagger api version parameter
    /// </summary>
    public class ApiVersionOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var actionMetaData = context.ApiDescription.ActionDescriptor.EndpointMetadata;
            operation.Parameters ??= new List<OpenApiParameter>();

            var apiVersionMetadata = actionMetaData.Any(item => item is ApiVersionMetadata);
            if (apiVersionMetadata)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "api-version",
                    In = ParameterLocation.Header,
                    Description = "Api version header value",
                    Schema = new OpenApiSchema
                    {
                        Type = "String",
                        Default = new OpenApiString("1.0")
                    }
                });
            }
        }
    }
}
