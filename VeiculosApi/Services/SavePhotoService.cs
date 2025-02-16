using System;

namespace VeiculosApi.Services;

public class SavePhotoService
{
    private readonly IWebHostEnvironment _environment;

    public SavePhotoService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<List<string>> SaveVehiclePhotosAsync(List<IFormFile> files)
    {
        var uploadFolder = Path.Combine(_environment.WebRootPath, "uploads", "vehicles-photos");

        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        List<string> savedPaths = new();

        foreach (var file in files)
        {
            if (file == null || file.Length == 0)
                continue;

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            savedPaths.Add($"/uploads/vehicles-photos/{uniqueFileName}");
        }

        return savedPaths;
    }
}
