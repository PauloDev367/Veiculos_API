using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeiculosApi.Exceptions;
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
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var data = await _service.GetAllAsync(page, pageSize);
        return Ok(new DefaultControllerResponse<PageResultResponse<Vehicle>>
        {
            Status = 200,
            Message = "Vehicles was founded",
            Data = data
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        try
        {
            await _service.DeleteOneAsync(id);
            return Ok(new DefaultControllerResponse<string>
            {
                Status = 200,
                Message = "Vehicles removed successfully",
            });
        }
        catch (ModelNotFoundException ex)
        {
            return NotFound(new DefaultControllerResponse<string>
            {
                Status = 404,
                Message = ex.Message,
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateVehicleRequest request)
    {
        try
        {
            var data = await _service.UpdateAsync(id, request);
            return Ok(new DefaultControllerResponse<Vehicle>
            {
                Status = 200,
                Message = "Vehicles updaated successfully",
                Data = data
            });
        }
        catch (ModelNotFoundException ex)
        {
            return NotFound(new DefaultControllerResponse<string>
            {
                Status = 404,
                Message = ex.Message,
            });
        }
    }

}
