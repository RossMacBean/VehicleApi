using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Vehicles.Api.Requests;
using Vehicles.Application;
using Vehicles.Application.Queries.Vehicles;
using Vehicles.Domain;
using Vehicles.Infrastructure.Repositories;

namespace Vehicles.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:ApiVersion}/[controller]")]
public class VehiclesController(
    VehicleQueryHandler queryHandler, 
    IVehiclesRepository vehiclesRepository, 
    ILogger<VehiclesController> logger)
    : ControllerBase
{
    [HttpGet("all")]
    [ProducesResponseType(typeof(List<Vehicle>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetAll()
    {
        var vehicles = vehiclesRepository.GetAll();
            
        if (vehicles.Count == 0)
        {
            return NoContent();
        }
            
        return Ok(vehicles);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<Vehicle>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Get([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        const int maxPageSize = 100;
        pageSize = Math.Min(maxPageSize, pageSize);
        
        var vehicles = vehiclesRepository.Get(page, pageSize);
            
        if (vehicles.Count == 0)
        {
            return NoContent();
        }
            
        return Ok(vehicles);
    }
    
    [HttpGet("Make/{make}")]
    [ProducesResponseType(typeof(List<Vehicle>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetByMake(string make)
    {
        var vehicles = vehiclesRepository.GetByMake(make);
            
        if (vehicles.Count == 0)
        {
            return NoContent();
        }
            
        return Ok(vehicles);
    }
    
    [HttpGet("Model/{model}")]
    [ProducesResponseType(typeof(List<Vehicle>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetByModel(string model)
    {
        var vehicles = vehiclesRepository.GetByModel(model);
            
        if (vehicles.Count == 0)
        {
            return NoContent();
        }
            
        return Ok(vehicles);
    }
    
    [HttpGet("Search/{searchTerm}")]
    [ProducesResponseType(typeof(List<Vehicle>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetBySearchTerm(string searchTerm)
    {
        var vehicles = vehiclesRepository.UnifiedSearch(searchTerm);
            
        if (vehicles.Count == 0)
        {
            return NoContent();
        }
            
        return Ok(vehicles);
    }
    
    // This endpoint demonstrates a simple example of using the CQRS pattern, which separates read and write logic into
    // queries and commands. You'd typically use the Mediator patter using a library like MediatR to dispatch queries
    // and commands to their handlers
    [HttpGet("query")]
    [ProducesResponseType(typeof(List<VehicleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Query(QueryVehiclesRequest request)
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult);
        }

        // In a more complex app, you could use MediatR to dispatch these queries to the appropriate handler
        // In a small simple app like this, that's overkill, so I just inject the handler directly and call it
        var vehicles = queryHandler.Handle(request.ToQuery());
        if (vehicles.Count == 0)
        {
            return NoContent();
        }
            
        return Ok(vehicles);
    }
}