using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeiculosApi.Http.Request;
using VeiculosApi.Http.Response;
using VeiculosApi.Models;
using VeiculosApi.Services;

namespace VeiculosApi.Controllers;

[Route("api/v1/vehicles")]
[ApiController]
public class VehicleController : ControllerBase
{
    private readonly VehicleService _service;

    public VehicleController(VehicleService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateVehicleRequest request)
    {
        var created = await _service.CreateAsync(request);
        return Ok(new DefaultControllerResponse<Vehicle>
        {
            Status = 200,
            Message = "Vehicle created successfully",
            Data = created,
        });
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOneAsync(Guid id)
    {
        var vehicle = await _service.GetOneAsync(id);
        if (vehicle == null)
        {
            return NotFound(new DefaultControllerResponse<string>
            {
                Status = 404,
                Message = "Vehicle was not funded"
            });
        }

        return Ok(new DefaultControllerResponse<Vehicle>
        {
            Status = 200,
            Message = "Vehicle was founded",
            Data = vehicle
        });
    }
}
