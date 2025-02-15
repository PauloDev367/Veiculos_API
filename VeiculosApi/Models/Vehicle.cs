using System;

namespace VeiculosApi.Models;

public class Vehicle
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Year { get; set; }
    public string Color { get; set; }
    public string FuelType { get; set; }
    public float Price { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public List<VehicleVariation> Variations { get; set; }
    public List<Photo> Photos { get; set; }
}
