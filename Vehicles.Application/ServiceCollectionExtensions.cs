using Microsoft.Extensions.DependencyInjection;
using Vehicles.Application.Queries.Vehicles;
using Vehicles.Infrastructure.Repositories;

namespace Vehicles.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<VehicleQueryHandler>();
        return services;
    }
}