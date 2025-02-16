using System;

namespace VeiculosApi.Http.Request;

public class UpdateVehicleRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Year { get; set; }
    public string? Color { get; set; }
    public string? FuelType { get; set; }
    public float Price { get; set; }
    public Guid CategoryId { get; set; }
}
