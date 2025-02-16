using Microsoft.AspNetCore.Mvc;
using VeiculosApi.Http.Request;
using VeiculosApi.Http.Response;
using VeiculosApi.Models;
using VeiculosApi.Services;

namespace VeiculosApi.Controllers;

[Route("api/v1/photos")]
[ApiController]
public class PhotoController : ControllerBase
{
    private readonly PhotoService _photoService;
    private readonly SavePhotoService _savePhotoService;

    public PhotoController(PhotoService photoService, SavePhotoService savePhotoService)
    {
        _photoService = photoService;
        _savePhotoService = savePhotoService;
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromForm] AddPhotosToVehicleRequest request)
    {
        var paths = await _savePhotoService.SaveVehiclePhotosAsync(request.Photos);
        var created = await _photoService.AddPhotoAsync(paths, request.VehicleId);

        return Ok(new DefaultControllerResponse<List<Photo>>
        {
            Status = 200,
            Message = "Photos added successfully",
            Data = created
        });
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveAsync(Guid id, [FromBody] RemovePhotoRequest request)
    {
        await _photoService.RemoveVehiclePhotosAsync(request.Ids, id);

        return Ok(new DefaultControllerResponse<string>
        {
            Status = 200,
            Message = "Photos removed successfully",
        });
    }
}