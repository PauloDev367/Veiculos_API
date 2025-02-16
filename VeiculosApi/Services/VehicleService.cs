using System;
using Microsoft.IdentityModel.Tokens;
using VeiculosApi.Data;
using VeiculosApi.Exceptions;
using VeiculosApi.Http.Request;
using VeiculosApi.Models;

namespace VeiculosApi.Services;

public class VehicleService
{
    private readonly AppDbContext _context;
    public VehicleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Vehicle> CreateAsync(CreateVehicleRequest request)
    {

        if (request.Name.IsNullOrEmpty())
        {
            throw new EmptyValueException("You need to informate the Name to continue");
        }
        if (request.Description.IsNullOrEmpty())
        {
            throw new EmptyValueException("You need to informate the Description to continue");
        }
        if (request.Year.IsNullOrEmpty())
        {
            throw new EmptyValueException("You need to informate the Year to continue");
        }
        if (request.Color.IsNullOrEmpty())
        {
            throw new EmptyValueException("You need to informate the Color to continue");
        }
        if (request.FuelType.IsNullOrEmpty())
        {
            throw new EmptyValueException("You need to informate the FuelType to continue");
        }
        if (request.Price <= 0)
        {
            throw new EmptyValueException("You need to informate the Price to continue");
        }
        if (request.CategoryId == Guid.Empty)
        {
            throw new EmptyValueException("You need to informate the CategoryId to continue");
        }

        var vehicle = new Vehicle
        {
            Name = request.Name,
            Description = request.Description,
            Year = request.Year,
            Color = request.Color,
            FuelType = request.FuelType,
            Price = request.Price,
            CategoryId = request.CategoryId,
        };

        await _context.AddAsync(vehicle);
        await _context.SaveChangesAsync();

        return vehicle;
    }
}
