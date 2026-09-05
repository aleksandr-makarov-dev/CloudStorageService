using Asp.Versioning;
using CloudStorage.Api.Middlewares;
using CloudStorage.Api.Options;
using CloudStorage.Infrastructure.Storage;

namespace CloudStorage.Api;

public static class DependencyInjection
{
    public static void AddApiLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddCors(configuration);
        services.AddProblemDetails();
        services.AddApiVersioning();
    }

    private static void AddCors(this IServiceCollection services, IConfiguration configuration)
    {
        var corsOptions = configuration
            .GetSection(CorsOptions.SectionName)
            .Get<CorsOptions>();

        ArgumentNullException.ThrowIfNull(corsOptions);

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(corsOptions.AllowedOrigins)
                    .WithHeaders(corsOptions.AllowedHeaders)
                    .WithMethods(corsOptions.AllowedMethods);

                if (corsOptions.AllowCredentials)
                {
                    builder.AllowCredentials();
                }
            });
        });
    }

    private static void AddProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = ctx =>
            {
                var httpContext = ctx.HttpContext;
                var problem = ctx.ProblemDetails;

                problem.Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}";
                problem.Extensions["timestamp"] = DateTimeOffset.UtcNow;
            };
        });
    }

    private static void AddApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
    }
}