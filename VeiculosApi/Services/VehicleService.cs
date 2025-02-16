using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VeiculosApi.Data;
using VeiculosApi.Exceptions;
using VeiculosApi.Http.Request;
using VeiculosApi.Http.Response;
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

    public async Task<Vehicle?> GetOneAsync(Guid id)
    {
        return await _context.Vehicles
            .AsNoTracking()
            .Include(x => x.Variations)
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<PageResultResponse<Vehicle>> GetAllAsync(int pageNumber, int pageSize)
    {
        if (pageSize > 10) pageSize = 10;

        var totalRecords = await _context.Categories.CountAsync();
        var vehicles = await _context.Vehicles
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new PageResultResponse<Vehicle>
        {
            TotalRecords = totalRecords,
            Data = vehicles,
            Page = pageNumber,
            PageSize = pageSize
        };

        return result;
    }

    public async Task DeleteOneAsync(Guid id)
    {
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        if (vehicle == null) throw new ModelNotFoundException("Vehicle not exists");

        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task<Vehicle> UpdateAsync(Guid id, UpdateVehicleRequest request)
    {

        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        if (vehicle == null) throw new ModelNotFoundException("Vehicle not exists");
        vehicle.Name = !request.Name.IsNullOrEmpty() ? request.Name : vehicle.Name;
        vehicle.Description = !request.Description.IsNullOrEmpty() ? request.Description : vehicle.Description;
        vehicle.Year = !request.Year.IsNullOrEmpty() ? request.Year : vehicle.Year;
        vehicle.Color = !request.Color.IsNullOrEmpty() ? request.Color : vehicle.Color;
        vehicle.FuelType = !request.FuelType.IsNullOrEmpty() ? request.FuelType : vehicle.FuelType;
        vehicle.Price = request.Price > 0 ? request.Price : vehicle.Price;
        vehicle.CategoryId = !(request.CategoryId == Guid.Empty) ? request.CategoryId : vehicle.CategoryId;

        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }
}
