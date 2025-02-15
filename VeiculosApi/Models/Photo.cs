using System;

namespace VeiculosApi.Models;

public class Photo
{
    public Guid Id { get; set; }
    public string Path { get; set; }
    public Vehicle Vehicle { get; set; }
    public Guid VehicleId { get; set; }
}
