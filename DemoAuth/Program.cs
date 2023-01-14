using Asp.Versioning;
using DemoAuth;
using DemoAuth.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// register services
builder.Services.AddScoped<IMovieService, MovieService>();

// register middleware
builder.Services.AddSwaggerGen(configuration);
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1.0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new HeaderApiVersionReader("api-version"),
        new QueryStringApiVersionReader("api-version"));
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(configuration);

var app = builder.Build();

app.UseSwagger();
app.MapMoviesEndpoints();
app.UseAuthenticationAndAuthorization();

app.Run();
