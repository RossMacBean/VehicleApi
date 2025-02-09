using Microsoft.AspNetCore.Mvc;
using Vehicles.Application.Queries;
using Vehicles.Application.Queries.Vehicles;

namespace Vehicles.Api.Requests;

public class QueryVehiclesRequest
{
    [FromQuery]
    public int PageNumber { get; set; } = 1;
    [FromQuery]
    public int PageSize { get; set; } = 10;
    
    [FromQuery]
    public decimal? MinPrice { get; set; }
    [FromQuery]
    public decimal? MaxPrice { get; set; }
    [FromQuery]
    public string? Make { get; set; }
    [FromQuery]
    public string? Model { get; set; }
    [FromQuery]
    public string? Trim { get; set; }
    [FromQuery]
    public string? Colour { get; set; }
    [FromQuery]
    public int? MinCo2Level { get; set; }
    [FromQuery]
    public int? MaxCo2Level { get; set; }
    [FromQuery]
    public string? Transmission { get; set; }
    [FromQuery]
    public string? FuelType { get; set; }
    [FromQuery]
    public int? MinEngineSize { get; set; }
    [FromQuery]
    public int? MaxEngineSize { get; set; }
    [FromQuery]
    public DateTime? MinDateFirstRegistered { get; set; }
    [FromQuery]
    public DateTime? MaxDateFirstRegistered { get; set; }
    [FromQuery]
    public int? MinMileage { get; set; }
    [FromQuery]
    public int? MaxMileage { get; set; }


    /// <summary>
    /// Validate this request
    /// </summary>
    /// <returns>A RequestValidationResult that contains details of any fields that failed validation</returns>
    public RequestValidationResult Validate()
    {
        var validationResult = new RequestValidationResult();
        
        if (MaxMileage.HasValue && MinMileage.HasValue && MaxMileage.Value < MinMileage.Value)
        {
            validationResult.AddError(nameof(MaxMileage),
                $"{nameof(MaxMileage)} must be greater than {nameof(MinMileage)}.");
        }
        
        if (MaxPrice.HasValue && MinPrice.HasValue && MaxPrice.Value < MinPrice.Value)
        {
            validationResult.AddError(nameof(MaxPrice), 
                $"{nameof(MaxPrice)} must be greater than {nameof(MinPrice)}.");

        }
        
        if (MaxDateFirstRegistered.HasValue && MinDateFirstRegistered.HasValue 
                                            && MaxDateFirstRegistered.Value < MinDateFirstRegistered.Value)
        {
            validationResult.AddError(nameof(MaxDateFirstRegistered),
                $"{nameof(MaxDateFirstRegistered)} must be greater than {nameof(MinDateFirstRegistered)}.");
        }

        return validationResult;
    }
    
    /// <summary>
    /// Transforms this request into a VehicleQuery that can be submitted to the business logic layer
    /// </summary>
    /// <returns>A VehicleQuery that represents this request</returns>
    public VehicleQuery ToQuery()
    {
        var query = new VehicleQuery()
            .WithPageNumber(PageNumber)
            .WithPageSize(PageSize)
            .WithPrice(MinPrice, MaxPrice)
            .WithMake(Make)
            .WithModel(Model)
            .WithTrim(Trim)
            .WithColour(Colour)
            .WithCo2Level(MinCo2Level, MaxCo2Level)
            .WithTransmission(Transmission)
            .WithFuelType(FuelType)
            .WithEngineSize(MinCo2Level, MaxCo2Level)
            .WithDateFirstRegistered(MinDateFirstRegistered, MaxDateFirstRegistered)
            .WithMileage(MinMileage, MaxMileage);
        
        return query;
    }
}