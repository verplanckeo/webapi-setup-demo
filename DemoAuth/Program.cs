using Asp.Versioning;
using DemoAuth;
using DemoAuth.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// register services
builder.Services.AddScoped<IMovieService, MovieService>();

// register middleware
builder.Services.AddSwaggerGen();
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

var app = builder.Build();

app.UseSwagger();
app.MapMoviesEndpoints();

app.Run();
