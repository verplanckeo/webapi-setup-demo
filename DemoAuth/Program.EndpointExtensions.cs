using Asp.Versioning.Builder;
using DemoAuth.Services;
using Microsoft.AspNetCore.Builder;

namespace DemoAuth
{
    /// <summary>
    /// Extension methods for API endpoints.
    /// </summary>
    public static class EndpointExtensions
    {
        private static ApiVersionSet? _versionSet;
        private static ApiVersionSet GetVersionSet(this WebApplication app)
        {
            if (_versionSet != null) return _versionSet;

            _versionSet = app.NewApiVersionSet()
                .HasApiVersion(new Asp.Versioning.ApiVersion(1.0))
                .HasApiVersion(new Asp.Versioning.ApiVersion(2.0))
                .ReportApiVersions()
                .Build();

            return _versionSet;
        }

        /// <summary>
        /// Register movie endpoints.
        /// </summary>
        /// <param name="app"></param>
        public static void MapMoviesEndpoints(this WebApplication app)
        {
            app.MapGet("/movies", (IMovieService service) =>
            {
                return Results.Ok(service.GetMovies());
            })
                .AllowAnonymous()
                .WithApiVersionSet(GetVersionSet(app))
                .MapToApiVersion(new Asp.Versioning.ApiVersion(1.0));
                

            app.MapDelete("/movies/{id}", (int id) =>
            {
                return Results.Ok($"Movie with id '{id}' has been removed.");
            })
                .RequireAuthorization()
                .WithApiVersionSet(GetVersionSet(app))
                .MapToApiVersion(new Asp.Versioning.ApiVersion(1.0));
        }
    }
}
