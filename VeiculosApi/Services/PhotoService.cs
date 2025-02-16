using System;
using VeiculosApi.Data;
using VeiculosApi.Http.Request;
using VeiculosApi.Models;

namespace VeiculosApi.Services;

public class PhotoService
{
    private readonly AppDbContext _context;
    public PhotoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Photo>> AddPhotoAsync(List<string> photoPaths, Guid vehicleId)
    {
        List<Photo> createdItems = new List<Photo>();
        foreach (var path in photoPaths)
        {
            var photo = new Photo
            {
                Path = path,
                VehicleId = vehicleId,
            };
            await _context.Photos.AddAsync(photo);
            createdItems.Add(photo);
        }

        await _context.SaveChangesAsync();
        return createdItems;
    }
}
