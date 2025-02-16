using System;

namespace VeiculosApi.Http.Request;

public class AddPhotosToVehicleRequest
{
    public List<IFormFile> Photos { get; set; }
    public Guid VehicleId { get; set; }
}
