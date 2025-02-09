using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Vehicles.Application.Queries.Vehicles;
using Vehicles.Domain;
using Vehicles.Infrastructure.Repositories;

namespace Vehicles.UnitTests.UnitTests;

[TestFixture]
public class VehicleQueryHandlerTests
{
    private VehicleQueryHandler _sut;
    private readonly Mock<IVehiclesRepository> _vehiclesRepository = new();
    private readonly Mock<ILogger<VehicleQueryHandler>> _logger = new();

    private List<Vehicle> _mockData = [];

    [SetUp]
    public void Setup()
    {
        _sut = new VehicleQueryHandler(_vehiclesRepository.Object, _logger.Object);
        _mockData =
        [
            new()
            {
                Make = "Toyota", Model = "Corolla", Price = 15000, Colour = "Red", Transmission = "Automatic",
                FuelType = "Petrol", Co2Level = 120, EngineSize = 1800, Mileage = 50000, DateFirstReg = "15/10/2020"
            },
            new()
            {
                Make = "Honda", Model = "Civic", Price = 18000, Colour = "Blue", Transmission = "Manual",
                FuelType = "Diesel", Co2Level = 110, EngineSize = 2000, Mileage = 30000, DateFirstReg = "10/06/2021"
            },
            new()
            {
                Make = "Ford", Model = "Focus", Price = 17000, Colour = "Black", Transmission = "Automatic",
                FuelType = "Hybrid", Co2Level = 100, EngineSize = 1000, Mileage = 20000, DateFirstReg = "20/05/2023"
            },
            new()
            {
                Make = "Mazda", Model = "5", Price = 15500, Colour = "Red", Transmission = "Manual",
                FuelType = "Hybrid", Co2Level = 100, EngineSize = 1600, Mileage = 50231, DateFirstReg = "19/04/2022"
            }
        ];
    }

    [Test]
    public void Handle_ShouldReturnAllVehicles_WhenNoFiltersApplied()
    {
        // Arrange
        SetupMockData();
        var query = new VehicleQuery();

        // Act
        var result = _sut.Handle(query);

        // Assert
        result.Count.Should().Be(_mockData.Count);
    }

    [Test]
    public void Handle_ShouldFilter_WhenFilterApplied()
    {
        // Arrange
        SetupMockData();
        var query = new VehicleQuery()
            .WithColour("Red")
            .WithMake("Mazda");

        // Act
        var result = _sut.Handle(query);

        // Assert
        result.Count.Should().Be(1);
        result[0].Colour.Should().Be("Red");
        result[0].Make.Should().Be("Mazda");
    }

    [Test]
    public void Handle_ShouldApplyPagination()
    {
        // Arrange
        SetupMockData();
        var query = new VehicleQuery()
            .WithPageNumber(2)
            .WithPageSize(2);

        // Act
        var result = _sut.Handle(query);

        // Assert
        result.Count.Should().Be(2);
        result[0].Make.Should().Be("Ford");
        result[0].Model.Should().Be("Focus");
    }

    private void SetupMockData()
    {
        _vehiclesRepository.Setup(x => x.Vehicles).Returns(_mockData.AsQueryable());
    }
}