using System.Reflection;
using System.Text.Json;
using Vehicles.Domain;

namespace Vehicles.Infrastructure.Repositories;

public class JsonVehiclesRepository : IVehiclesRepository
{
    private readonly List<Vehicle> _vehicles;
    
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };

    public JsonVehiclesRepository()
    {
        var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        using var r = new StreamReader(Path.Join(basePath, "Repositories/vehicles.json"));
        var json = r.ReadToEnd();
        _vehicles = JsonSerializer.Deserialize<List<Vehicle>>(json, JsonSerializerOptions) ?? [];
    }
    
    public IQueryable<Vehicle> Vehicles => _vehicles.AsQueryable();
    
    public List<Vehicle> GetAll() => _vehicles;
    
    public List<Vehicle> Get(int pageNumber, int pageSize)
    {
        return _vehicles
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public List<Vehicle> GetByMake(string make)
    {
        return _vehicles
            .Where(x => string.Equals(x.Make, make, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }

    public List<Vehicle> GetByModel(string model)
    {
        return _vehicles
            .Where(x => string.Equals(x.Model, model, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    
    // A Naive implementation of a unified search. Only supports text fields.
    // In a real system you might implement this by using a more robust solution 
    // like making use of SQL Server's Full-Text Search functionality, or even a separate system like Elasticsearch
    public List<Vehicle> UnifiedSearch(string term)
    {
        if (string.IsNullOrEmpty(term))
        {
            return _vehicles.ToList();
        }

        return _vehicles
            .Where(v => v.Make.Contains(term, StringComparison.InvariantCultureIgnoreCase)
                        || v.Model.Contains(term, StringComparison.InvariantCultureIgnoreCase)
                        || v.Trim.Contains(term, StringComparison.InvariantCultureIgnoreCase)
                        || v.Colour.Contains(term, StringComparison.InvariantCultureIgnoreCase)
                        || v.Transmission.Contains(term, StringComparison.InvariantCultureIgnoreCase)
                        || v.FuelType.Contains(term, StringComparison.InvariantCultureIgnoreCase))

            .ToList();
    }
}