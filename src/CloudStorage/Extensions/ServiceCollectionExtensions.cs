using CloudStorage.HostedServices;
using CloudStorage.Options;
using CloudStorage.Persistence;
using CloudStorage.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Minio;

namespace CloudStorage.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddOptions(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<MinioOptions>()
            .Bind(configuration.GetSection(MinioOptions.SectionName))
            .ValidateOnStart();

        services
            .AddOptions<StorageOptions>()
            .Bind(configuration.GetSection(StorageOptions.SectionName))
            .ValidateOnStart();

        services.AddOptions<CorsOptions>()
            .Bind(configuration.GetSection(CorsOptions.SectionName))
            .ValidateOnStart();
    }

    public static void AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));
    }

    public static void AddMinioStorage(this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration
            .GetSection(MinioOptions.SectionName)
            .Get<MinioOptions>();

        ArgumentNullException.ThrowIfNull(options);

        services.AddMinio(client => client
            .WithEndpoint(options.Endpoint)
            .WithCredentials(options.AccessKey, options.SecretKey)
            .WithSSL(options.UseSsl)
            .Build());

        services.AddHostedService<MinioStartupHostedService>();
    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IResourceService, ResourceService>();
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(
            typeof(ServiceCollectionExtensions).Assembly);
    }

    public static void AddCors(this IServiceCollection services, IConfiguration configuration)
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
}