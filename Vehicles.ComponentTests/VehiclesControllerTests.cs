using NUnit.Framework;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http.Extensions;
using Vehicles.Domain;

namespace Vehicles.ComponentTests;

[TestFixture]
public class VehiclesControllerTests
{
    private ComponentTestFixture _fixture = null!;

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _fixture = ComponentTestFixture.Create();
    }

    [Test]
    public async Task GetAll_ShouldReturnAllVehicles()
    {
        // Arrange
        
        // Act
        var response = await _fixture.HttpClient.GetAsync("/v1/vehicles/all");
        
        //Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<Vehicle>>(json, _options);
        result.Should().NotBeNull()
            .And.NotBeEmpty()
            .And.BeOfType<List<Vehicle>>();
    }
    
    [Test]
    public async Task GetVehicles_ShouldFilterByPage()
    {
        // Arrange
        var queryBuilder = new QueryBuilder { { "page", "2" }, { "pageSize", "50" } };
        
        // Act
        var response = await _fixture.HttpClient.GetAsync($"/v1/vehicles{queryBuilder.ToQueryString()}");
        
        //Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<Vehicle>>(json, _options);
        result.Should().NotBeNull()
            .And.NotBeEmpty()
            .And.HaveCount(50);
    }
    
    [Test] 
    [TestCase("Toyota")]
    [TestCase("mazda")]
    [TestCase("BMW")]
    public async Task GetByMake_ShouldFilterByMake(string make)
    {
        // Arrange
        var uri = $"/v1/vehicles/make/{make}";
        
        // Act
        var response = await _fixture.HttpClient.GetAsync(uri);
        
        //Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<Vehicle>>(json, _options);
        result.Should().NotBeNull()
            .And.NotBeEmpty()
            .And.AllSatisfy(v =>
        {
            v.Make.Should().BeEquivalentTo(make);
        });
    }
    
    [Test] 
    [TestCase("ASTRA")]
    [TestCase("focus")]
    [TestCase("E class")]
    public async Task GetByModel_ShouldFilterByModel(string model)
    {
        // Arrange
        var uri = $"/v1/vehicles/model/{model}";
        
        // Act
        var response = await _fixture.HttpClient.GetAsync(uri);
        
        //Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<Vehicle>>(json, _options);
        result.Should().NotBeNull()
            .And.NotBeEmpty()
            .And.AllSatisfy(v =>
            {
                v.Model.Should().BeEquivalentTo(model);
            });
    }
    
    [Test]
    public async Task GetBySearchTerm_ShouldFilterByColour()
    {
        // Arrange
        const string uri = $"/v1/vehicles/search/red";
        
        // Act
        var response = await _fixture.HttpClient.GetAsync(uri);
        
        //Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<Vehicle>>(json, _options);
        result.Should().NotBeNull()
            .And.NotBeEmpty()
            .And.AllSatisfy(v =>
            {
                v.Colour.Should().ContainEquivalentOf("red");
            });
    }
    
    [Test]
    public async Task GetBySearchTerm_ShouldFilterByFuelType()
    {
        // Arrange
        const string uri = $"/v1/vehicles/search/Unleaded";
        
        // Act
        var response = await _fixture.HttpClient.GetAsync(uri);
        
        //Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<Vehicle>>(json, _options);
        result.Should().NotBeNull()
            .And.NotBeEmpty()
            .And.AllSatisfy(v =>
            {
                v.FuelType.Should().ContainEquivalentOf("Unleaded");
            });
    }
    
    [Test]
    public async Task GetBySearchQuery_ShouldHaveDefaultPageSize()
    {
        // Arrange
        const string uri = $"/v1/vehicles/query";
        
        // Act
        var response = await _fixture.HttpClient.GetAsync(uri);
        
        //Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<Vehicle>>(json, _options);
        result.Should().NotBeNull()
            .And.HaveCount(10);
    }
    
    [Test]
    [TestCase(5000, 10000)]
    [TestCase(5000, 25000)]
    [TestCase(15000, 20000)]
    public async Task GetBySearchQuery_ShouldFilterByPrice(int minPrice, int maxPrice)
    {
        // Arrange
        var queryBuilder = new QueryBuilder { { "minPrice", $"{minPrice}" }, { "maxPrice", $"{maxPrice}" } };
        var uri = $"/v1/vehicles/query{queryBuilder.ToQueryString()}";
        
        // Act
        var response = await _fixture.HttpClient.GetAsync(uri);
        
        //Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<Vehicle>>(json, _options);
        result.Should().NotBeNull()
            .And.NotBeEmpty()
            .And.AllSatisfy(v =>
            {
                v.Price.Should().BeGreaterOrEqualTo(minPrice)
                    .And.BeLessOrEqualTo(maxPrice);
            });
    }
    
    [Test]
    public async Task GetBySearchQuery_ShouldFilterByDateFirstRegistered()
    {
        // Arrange
        var minDate = new DateTime(2015, 1, 1);
        var maxDate = new DateTime(2020, 12, 31);
        var queryBuilder = new QueryBuilder
        {
            { "minDateFirstRegistered", $"{minDate:O}" }, 
            { "maxDateFirstRegistered", $"{maxDate:O}" }
        };
        var uri = $"/v1/vehicles/query{queryBuilder.ToQueryString()}";
        
        // Act
        var response = await _fixture.HttpClient.GetAsync(uri);
        
        //Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<Vehicle>>(json, _options);
        result.Should().NotBeNull()
            .And.NotBeEmpty()
            .And.AllSatisfy(v =>
            {
                var actualDate = DateTime.Parse(v.DateFirstReg);
                actualDate.Should().BeAfter(minDate)
                    .And.BeBefore(maxDate);
            });
    }
    
    [Test]
    public async Task GetBySearchQuery_ShouldFilterByMultipleFields()
    {
        // Arrange
        const int minEngineSize = 1600;
        const int maxEngineSize = 2000;
        const string colour = "black";
        const string transmission = "MANUAL";
        var queryBuilder = new QueryBuilder
        {
            { "minEngineSize", $"{minEngineSize}" }, 
            { "maxEngineSize", $"{maxEngineSize}" },
            { "colour", colour },
            { "transmission", transmission }
        };
        var uri = $"/v1/vehicles/query{queryBuilder.ToQueryString()}";
        
        // Act
        var response = await _fixture.HttpClient.GetAsync(uri);
        
        //Assert
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<Vehicle>>(json, _options);
        result.Should().NotBeNull()
            .And.NotBeEmpty()
            .And.AllSatisfy(v =>
            {
                v.EngineSize.Should().BeInRange(minEngineSize, maxEngineSize);
                v.Colour.Should().ContainEquivalentOf(colour);
                v.Transmission.Should().Be(transmission);
            });
    }
}