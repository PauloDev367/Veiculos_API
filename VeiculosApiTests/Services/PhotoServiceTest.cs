using Microsoft.EntityFrameworkCore;
using VeiculosApi.Data;
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
}
