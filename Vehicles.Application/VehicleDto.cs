using Vehicles.Application.Exceptions;
using Vehicles.Domain;

namespace Vehicles.Application;

public record VehicleDto(
    decimal Price,
    string Make,
    string Model,
    string Trim,
    string Colour,
    int Co2Level,
    string Transmission,
    string FuelType,
    int EngineSize,
    DateTime DateFirstReg,
    int Mileage)
{
    public static VehicleDto FromDomainModel(Vehicle vehicle)
    {
        if (!DateTime.TryParse(vehicle.DateFirstReg, out var parsedDateFirstReg))
        {
            throw new DomainMappingException(typeof(Vehicle), typeof(VehicleDto), nameof(Vehicle.DateFirstReg),
                "Not a valid date string");
        }
        
        return new VehicleDto(
            vehicle.Price,
            vehicle.Make,
            vehicle.Model,
            vehicle.Trim,
            vehicle.Colour,
            vehicle.Co2Level,
            vehicle.Transmission,
            vehicle.FuelType,
            vehicle.EngineSize,
            parsedDateFirstReg,
            vehicle.Mileage
        );
    }
}

