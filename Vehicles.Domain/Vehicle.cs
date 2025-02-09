namespace Vehicles.Domain;

public class Vehicle
{
    public decimal Price { get; set; } 
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Trim { get; set; } = string.Empty;
    public string Colour { get; set; } = string.Empty;
    public int Co2Level { get; set; }
    public string Transmission { get; set; } = string.Empty;
    public string FuelType { get; set; } = string.Empty;
    public int EngineSize { get; set; }
    public string DateFirstReg { get; set; } = string.Empty;
    public int Mileage { get; set; }
}