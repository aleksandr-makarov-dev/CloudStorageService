using CloudStorage.Options;
using CloudStorage.Persistence;
using CloudStorage.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Minio;

namespace CloudStorage.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IResourceService, ResourceService>();
    }

    public static void AddMinioStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<MinioOptions>(MinioOptions.SectionName);

        var options = configuration
            .GetSection(MinioOptions.SectionName)
            .Get<MinioOptions>();

        ArgumentNullException.ThrowIfNull(options);

        services.AddMinio(client => client
            .WithEndpoint(options.Endpoint)
            .WithCredentials(options.AccessKey, options.SecretKey)
            .WithSSL(options.UseSsl)
            .Build());
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
    }
}