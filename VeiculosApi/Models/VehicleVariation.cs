using System;

namespace VeiculosApi.Models;

public class VehicleVariation
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public float Price { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
}
