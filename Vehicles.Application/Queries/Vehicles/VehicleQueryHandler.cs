using System.Text.Json;
using Microsoft.Extensions.Logging;
using Vehicles.Infrastructure.Repositories;

namespace Vehicles.Application.Queries.Vehicles;

// This class demonstrates what a query handler that performs read only operations in the CQRS pattern
// If using a library like MediatR, this could be registered as an IRequestHandler and calls to it could be
// dispatched to it by MediatR, rather than being called directly
public class VehicleQueryHandler(
    IVehiclesRepository vehiclesRepository, 
    ILogger<VehicleQueryHandler> logger)
{
    public List<VehicleDto> Handle(VehicleQuery query)
    {
        logger.LogDebug("Handling vehicle query {Query}", JsonSerializer.Serialize(query));
        
        var queryResult = vehiclesRepository.Vehicles;
       
        queryResult = queryResult.Where(x => query.Price.IsInRange(x.Price));
        
        if (query.Make is not null)
        {
            queryResult = queryResult
                .Where(x => string.Equals(x.Make, query.Make, StringComparison.InvariantCultureIgnoreCase));
        }

        if (query.Model is not null)
        {
            queryResult = queryResult
                .Where(x => string.Equals(x.Model, query.Model, StringComparison.InvariantCultureIgnoreCase));
        }

        if (query.Trim is not null)
        {
            queryResult = queryResult
                .Where(x => string.Equals(x.Model, query.Trim, StringComparison.InvariantCultureIgnoreCase));
        }

        if (query.Colour is not null)
        {
            queryResult = queryResult
                .Where(x => string.Equals(x.Colour, query.Colour, StringComparison.InvariantCultureIgnoreCase));
        }
        
        queryResult = queryResult.Where(x => query.Co2Level.IsInRange(x.Co2Level));

        if (query.Transmission is not null)
        {
            queryResult = queryResult
                .Where(x => string.Equals(x.Transmission, query.Transmission, StringComparison.InvariantCultureIgnoreCase));
        }

        if (query.FuelType is not null)
        {
            queryResult = queryResult
                .Where(x => string.Equals(x.FuelType, query.FuelType, StringComparison.InvariantCultureIgnoreCase));
        }
        
        queryResult = queryResult.Where(x => query.EngineSize.IsInRange(x.EngineSize));
        queryResult = queryResult.Where(x => query.Mileage.IsInRange(x.Mileage));
        
        // Because DateFirstReg is stored as a string, and you can't do Date.Parse in an Expression Tree
        // (which is what IQueryable uses), we have to convert to an IEnumerable to be able to run this block
        queryResult = queryResult.AsEnumerable().Where(x =>
        {
            if (!DateTime.TryParse(x.DateFirstReg, out var firstRegDate))
            {
                // If the datetime can't be parsed then we have bad data
                // In a production system this would be a real red flag that would need logging somewhere as
                // it would indicate potential data corruption that would need to be investigated
                // For this simple API, just ignore any records where this is the case
                logger.LogError("Unable to parse date for the vehicle {Make} {Model} {Colour}. Dates are expected to" +
                                "be in the format \"DD/MM/YYYY\", but found {DateString}", x.Make, x.Model, x.Colour, x.DateFirstReg);
                return false;
            }
            
            return query.DateFirstRegistered.IsInRange(firstRegDate);
        }).AsQueryable();
        
        return queryResult
            .Skip((query.PageQuery.PageNumber - 1) * query.PageQuery.PageSize)
            .Take(query.PageQuery.PageSize)
            .Select(v => VehicleDto.FromDomainModel(v))
            .ToList();
    }

}