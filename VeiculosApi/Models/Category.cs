using System;

namespace VeiculosApi.Models;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Vehicle> Vehicles { get; set; }
}
