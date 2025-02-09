namespace Vehicles.Application.Queries.Vehicles;

public class VehicleQuery
{
    private const int DefaultPageSize = 50;
    private const int MaxPageSize = 100;
    internal PageQuery PageQuery { get; private set; } = new (1, DefaultPageSize);
    
    internal RangeQuery<decimal> Price { get; private set; } = new (0M, decimal.MaxValue);
    internal string? Make { get; private set; }
    internal string? Model { get; private set; }
    internal string? Trim { get; private set; }
    internal string? Colour { get; private set; }
    internal RangeQuery<int> Co2Level { get; private set; } = new (0, int.MaxValue);
    internal string? Transmission { get; private set; }
    internal string? FuelType { get; private set; }
    internal RangeQuery<int> EngineSize { get; private set; } = new(0, int.MaxValue);
    internal RangeQuery<DateTime> DateFirstRegistered { get; private set; } = new (DateTime.MinValue, DateTime.MaxValue);
    internal RangeQuery<int> Mileage { get; private set;  } = new(0, int.MaxValue);

    public VehicleQuery WithPageNumber(int? page)
    {
        PageQuery = PageQuery.WithPageNumber(page);
        return this;
    }

    public VehicleQuery WithPageSize(int? pageSize)
    {
        PageQuery = PageQuery.WithPageSize(pageSize.HasValue ? Math.Min(pageSize.Value, MaxPageSize) : null);
                    
        return this;
    }

    public VehicleQuery WithMake(string? make)
    {
        Make = make;
        return this;
    }

    public VehicleQuery WithModel(string? model)
    {
        Model = model;
        return this;
    }

    public VehicleQuery WithTrim(string? trim)
    {
        Trim = trim;
        return this;
    }
    
    public VehicleQuery WithColour(string? colour)
    {
        Colour = colour;
        return this;
    }

    public VehicleQuery WithCo2Level(int? min, int? max)
    {
        if (min.HasValue)
        {
            Co2Level = Co2Level.WithMin(min.Value);
        }

        if (max.HasValue)
        {
            Co2Level = Co2Level.WithMax(max.Value);
        }
        
        return this;
    }

    public VehicleQuery WithTransmission(string? transmission)
    {
        Transmission = transmission;
        return this;
    }

    public VehicleQuery WithFuelType(string? fuelType)
    {
        FuelType = fuelType;
        return this;
    }

    public VehicleQuery WithEngineSize(int? min, int? max)
    {
        if (min.HasValue)
        {
            EngineSize = EngineSize.WithMin(min.Value);
        }

        if (max.HasValue)
        {
            EngineSize = EngineSize.WithMax(max.Value);
        }
        
        return this;
    }
    
    public VehicleQuery WithDateFirstRegistered(DateTime? min, DateTime? max)
    {
        if (min.HasValue)
        {
            DateFirstRegistered = DateFirstRegistered.WithMin(min.Value);
        }

        if (max.HasValue)
        {
            DateFirstRegistered = DateFirstRegistered.WithMax(max.Value);
        }

        return this;
    }
    
    public VehicleQuery WithPrice(decimal? min, decimal? max)
    {
        if (min.HasValue)
        {
            Price = Price.WithMin(min.Value);
        }

        if (max.HasValue)
        {
            Price = Price.WithMax(max.Value);
        }

        return this;
    }
    
    public VehicleQuery WithMileage(int? min, int? max)
    {
        if (min.HasValue)
        {
            Mileage = Mileage.WithMin(min.Value);
        }

        if (max.HasValue)
        {
            Mileage = Mileage.WithMax(max.Value);
        }

        return this;
    }
}