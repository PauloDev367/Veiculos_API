using Microsoft.EntityFrameworkCore;
using VeiculosApi.Data;
using VeiculosApi.Models;
using VeiculosApi.Services;

namespace VeiculosApiTests.Services;

public class PhotoServiceTest
{
    private readonly AppDbContext _memoryDbContext;
    private readonly PhotoService _service;
    public PhotoServiceTest()
    {
        _memoryDbContext = CreateInMemoryDbContext();
        _service = new PhotoService(_memoryDbContext);
    }

    private AppDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task ShouldAddPhotosToDatabase()
    {
        var vehicleId = Guid.NewGuid();
        var photoPaths = new List<string>
            {
                "uploads/photo1.jpg",
                "uploads/photo2.jpg"
            };

        var result = await _service.AddPhotoAsync(photoPaths, vehicleId);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        foreach (var photo in result)
        {
            Assert.Contains(photo.Path, photoPaths);
            Assert.Equal(vehicleId, photo.VehicleId);
        }

        var photosInDb = await _memoryDbContext.Photos.ToListAsync();
        Assert.Equal(2, photosInDb.Count);
    }
    [Fact]
    public async Task ShouldRemovePhotosFromDatabase()
    {
        var vehicleId = Guid.NewGuid();
        var photo1 = new Photo { Id = Guid.NewGuid(), VehicleId = vehicleId, Path = "uploads/photo1.jpg" };
        var photo2 = new Photo { Id = Guid.NewGuid(), VehicleId = vehicleId, Path = "uploads/photo2.jpg" };

        await _memoryDbContext.Photos.AddRangeAsync(photo1, photo2);
        await _memoryDbContext.SaveChangesAsync();

        var idsToRemove = new List<Guid> { photo1.Id, photo2.Id };

        await _service.RemoveVehiclePhotosAsync(idsToRemove, vehicleId);

        var photosInDb = await _memoryDbContext.Photos.ToListAsync();
        Assert.Empty(photosInDb);
    }

    [Fact]
    public async Task ShouldNotFailIfPhotoDoesNotExist()
    {
        var vehicleId = Guid.NewGuid();
        var idsToRemove = new List<Guid> { Guid.NewGuid() };

        await _service.RemoveVehiclePhotosAsync(idsToRemove, vehicleId);

        var photosInDb = await _memoryDbContext.Photos.ToListAsync();
        Assert.Empty(photosInDb);
    }

}
