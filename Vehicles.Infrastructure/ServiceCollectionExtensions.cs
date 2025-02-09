using Microsoft.Extensions.DependencyInjection;
using Vehicles.Infrastructure.Repositories;

namespace Vehicles.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Normally I'd register repositories as scoped if I was using EF so that each API request got it's own
        // repository instance and DbContext, which are also registered as scoped
        // However because this repository loads a rather large JSON file from disk that never changes, it's much
        // more efficient to only load the repository once, therefore it's registered as a singleton
        services.AddSingleton<IVehiclesRepository, JsonVehiclesRepository>();
        return services;
    }
}