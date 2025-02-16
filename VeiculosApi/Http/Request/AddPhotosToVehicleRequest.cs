using System;
using System.ComponentModel.DataAnnotations;

namespace VeiculosApi.Http.Request;

public class AddPhotosToVehicleRequest
{
    [Required]
    public List<IFormFile> Photos { get; set; }
    [Required]
    public Guid VehicleId { get; set; }
}
