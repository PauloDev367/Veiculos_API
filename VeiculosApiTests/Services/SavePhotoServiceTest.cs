using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using VeiculosApi.Services;

namespace VeiculosApiTests.Services;

public class SavePhotoServiceTest
{
    private readonly SavePhotoService _photoService;
    private readonly Mock<IWebHostEnvironment> _mockEnvironment;
    private readonly string _testUploadPath;

    public SavePhotoServiceTest()
    {
        _mockEnvironment = new Mock<IWebHostEnvironment>();
        _testUploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        _mockEnvironment.Setup(env => env.WebRootPath).Returns(_testUploadPath);

        _photoService = new SavePhotoService(_mockEnvironment.Object);
    }

    [Fact]
    public async Task SaveVehiclePhotosAsync_ShouldSaveFilesAndReturnPaths()
    {
        var files = new List<IFormFile>
        {
            CreateMockFile("test1.jpg", "image/jpeg"),
            CreateMockFile("test2.png", "image/png")
        };

        var result = await _photoService.SaveVehiclePhotosAsync(files);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        foreach (var filePath in result)
        {
            Assert.Contains("/uploads/vehicles-photos/", filePath);
        }

        foreach (var path in result)
        {
            var fullPath = Path.Combine(_testUploadPath, path.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }

    private IFormFile CreateMockFile(string fileName, string contentType)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write("Dummy file content");
        writer.Flush();
        stream.Position = 0;

        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns(fileName);
        mockFile.Setup(f => f.Length).Returns(stream.Length);
        mockFile.Setup(f => f.OpenReadStream()).Returns(stream);
        mockFile.Setup(f => f.ContentType).Returns(contentType);

        return mockFile.Object;
    }
}
