using Vehicles.Domain;

namespace Vehicles.Infrastructure.Repositories;

public interface IVehiclesRepository
{
    IQueryable<Vehicle> Vehicles { get; }

    List<Vehicle> GetAll();
    List<Vehicle> Get(int pageNumber, int pageSize);
    List<Vehicle> GetByMake(string make);
    List<Vehicle> GetByModel(string model);
    List<Vehicle> UnifiedSearch(string term);
}