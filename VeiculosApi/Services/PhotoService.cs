using System;
using Microsoft.EntityFrameworkCore;
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
    public async Task RemoveVehiclePhotosAsync(List<Guid> ids, Guid vehicleId)
    {
        foreach (var photoId in ids)
        {
            var photo = await _context.Photos
                .FirstOrDefaultAsync(x => x.Id.Equals(photoId));

            if (photo != null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", photo.Path.TrimStart('/'));
                Console.WriteLine($"Tentando deletar: {filePath}");

                if (System.IO.File.Exists(filePath))
                {
                    Console.WriteLine($"Arquivo encontrado, deletando...");
                    System.IO.File.Delete(filePath);
                }
                else
                {
                    Console.WriteLine($"Arquivo não encontrado: {filePath}");
                }

                _context.Remove(photo);
            }
        }

        await _context.SaveChangesAsync();
    }


}
