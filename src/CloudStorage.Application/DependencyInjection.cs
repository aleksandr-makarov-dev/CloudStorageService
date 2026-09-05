using Microsoft.Extensions.DependencyInjection;

namespace CloudStorage.Application;

public static class DependencyInjection
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediator(options => { options.ServiceLifetime = ServiceLifetime.Scoped; });
    }
}