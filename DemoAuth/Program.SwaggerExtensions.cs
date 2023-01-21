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
        /// <param name="configuration"></param>
        public static void AddSwaggerGen(this IServiceCollection services, ConfigurationManager configuration)
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

                var authUrl = $"{configuration["Auth:Instance"]}{configuration["Auth:TenantId"]}/oauth2/v2.0";
                var authorizationUrl = $"{authUrl}/authorize";
                var tokenUrl = $"{authUrl}/token";

                var scheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(authorizationUrl),
                            TokenUrl = new Uri(tokenUrl)
                        }
                    },
                    Type = SecuritySchemeType.OAuth2
                };

                c.AddSecurityDefinition("OAuth", scheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Id = "OAuth", Type = ReferenceType.SecurityScheme }
                        },
                        Array.Empty<string>()
                    }
                });
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

                c.EnablePersistAuthorization();

                c.OAuthClientId(app.Configuration["Auth:Swagger:ClientId"]);
                c.OAuthScopes(app.Configuration.GetSection("Auth:Swagger:Scopes").Get<string[]>());
                c.OAuthUsePkce();
            });
        }
    }
}
