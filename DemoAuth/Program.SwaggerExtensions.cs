using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace DemoAuth
{
    /// <summary>
    /// Extension methods for Swagger configuration.
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Register swagger output.
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                //This is to solve the Swagger "Can't use schemaId for type ... the same schemaId is already used for type" issue.
                //When using 2 variables with the same name in different namespaces, it tries to create a schemaId for both namespaces but with the same value.
                //Example: we have 2 api calls with the parameter "request", swagger will generate a schemaId called "$request" for both calls.
                //But because of this, it'll conflict in the generation, schemaId's have to be unique. Hence we do type.ToString(), this includes the namespace in the schemaid generation.
                c.CustomSchemaIds(type => type.ToString());

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Swagger for DemoAuth",
                    Version = "v1",
                    Contact = new OpenApiContact { Email = "olivier@itigai.com" }
                });

                c.OperationFilter<ApiVersionOperationFilter>();

                var xmlDocumentation = Path.Combine(AppContext.BaseDirectory, "DemoAuth.xml");
                c.IncludeXmlComments(xmlDocumentation);
            });
        }

        /// <summary>
        /// Configure swagger UI
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwagger(this WebApplication app)
        {
            app.UseSwagger(options =>
            {
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"Demo v1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
