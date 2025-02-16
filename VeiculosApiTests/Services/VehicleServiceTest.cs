
using Microsoft.EntityFrameworkCore;
using VeiculosApi.Data;
using VeiculosApi.Exceptions;
using VeiculosApi.Http.Request;
using VeiculosApi.Models;
using VeiculosApi.Services;

namespace VeiculosApiTests.Services;
public class VehicleServiceTest
{
    private AppDbContext _memoryDbContext;
    private VehicleService _service;

    public VehicleServiceTest()
    {
        var dbContext = CreateInMemoryDbContext();
        _memoryDbContext = dbContext;
        _service = new VehicleService(_memoryDbContext);
    }

    private AppDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        return new AppDbContext(options);
    }

    [Theory]
    [InlineData("You need to informate the Name to continue", "", "Description", "2020", "red", "Flex", 30, true)]
    [InlineData("You need to informate the Description to continue", "Nome", "", "2020", "red", "Flex", 30, true)]
    [InlineData("You need to informate the Year to continue", "Nome", "Description", "", "red", "Flex", 30, true)]
    [InlineData("You need to informate the Color to continue", "Nome", "Description", "2020", "", "Flex", 30, true)]
    [InlineData("You need to informate the FuelType to continue", "Nome", "Description", "2020", "red", "", 0, true)]
    [InlineData("You need to informate the Price to continue", "Nome", "Description", "2020", "red", "Flex", 0, true)]
    [InlineData("You need to informate the CategoryId to continue", "Nome", "Description", "2020", "red", "Flex", 30, false)]
    public async Task ShouldNotCreateAVehicleIfDataIsNotCorrect(
        string expectedMessage,
        string Name,
        string Description,
        string Year,
        string Color,
        string FuelType,
        float Price,
        bool CategoryId
        )
    {
        var vehicleRequest = new CreateVehicleRequest
        {
            Name = Name,
            Description = Description,
            Year = Year,
            Color = Color,
            FuelType = FuelType,
            Price = Price,
            CategoryId = CategoryId == true ? Guid.NewGuid() : Guid.Empty,
        };

        var exception = await Assert.ThrowsAsync<EmptyValueException>(() => _service.CreateAsync(vehicleRequest));
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task ShouldCreateAVehicleIfAllDataIsCorrect()
    {
        var vehicleRequest = new CreateVehicleRequest
        {
            Name = "Name",
            Description = "Description",
            Year = "Year",
            Color = "Color",
            FuelType = "FuelType",
            Price = 200,
            CategoryId = Guid.NewGuid(),
        };

        var created = await _service.CreateAsync(vehicleRequest);
        Assert.Equal(created.Name, vehicleRequest.Name);
        Assert.NotNull(created.Id);
    }

    [Fact]
    public async Task ShouldGetOneVehicleIfItExists()
    {
        var vehicleRequest = new CreateVehicleRequest
        {
            Name = "Name",
            Description = "Description",
            Year = "Year",
            Color = "Color",
            FuelType = "FuelType",
            Price = 200,
            CategoryId = Guid.NewGuid(),
        };

        var created = await _service.CreateAsync(vehicleRequest);
        var search = await _service.GetOneAsync(created.Id);

        Assert.NotNull(search);
    }
    [Fact]
    public async Task ShouldNotGetOneVehicleIfItNotExists()
    {
        var search = await _service.GetOneAsync(Guid.NewGuid());
        Assert.Null(search);
    }

    [Fact]
    public async Task ShouldGetAllVehicles()
    {
        var vehicleRequest = new CreateVehicleRequest
        {
            Name = "Name",
            Description = "Description",
            Year = "Year",
            Color = "Color",
            FuelType = "FuelType",
            Price = 200,
            CategoryId = Guid.NewGuid(),
        };

        await _service.CreateAsync(vehicleRequest);
        var pageNumber = 1;
        var perPage = 1;

        var search = await _service.GetAllAsync(pageNumber, perPage);
        var totalExpected = 1;
        Assert.Equal(totalExpected, search.Data.Count());
    }

    [Fact]
    public async Task ShouldSetPerPageEqual10IfPassedPerPageIsBiggerThan10WhenAllVehicles()
    {
        var vehicleRequest = new CreateVehicleRequest
        {
            Name = "Name",
            Description = "Description",
            Year = "Year",
            Color = "Color",
            FuelType = "FuelType",
            Price = 200,
            CategoryId = Guid.NewGuid(),
        };

        await _service.CreateAsync(vehicleRequest);
        var pageNumber = 1;
        var perPage = 11;

        var search = await _service.GetAllAsync(pageNumber, perPage);
        var totalExpected = 10;
        Assert.Equal(totalExpected, search.PageSize);
    }

    [Fact]
    public async Task ShouldDeleteAVehicleIfItExists()
    {
        var vehicleRequest = new CreateVehicleRequest
        {
            Name = "Name",
            Description = "Description",
            Year = "Year",
            Color = "Color",
            FuelType = "FuelType",
            Price = 200,
            CategoryId = Guid.NewGuid(),
        };

        var created = await _service.CreateAsync(vehicleRequest);
        await _service.DeleteOneAsync(created.Id);

        var search = await _service.GetOneAsync(created.Id);
        Assert.Null(search);
    }
    [Fact]
    public async Task ShouldNotDeleteAVehicleIfItNotExists()
    {
        var exception = await Assert.ThrowsAsync<ModelNotFoundException>(() => _service.DeleteOneAsync(Guid.NewGuid()));
        var expectedMessage = "Vehicle not exists";
        Assert.Equal(expectedMessage, exception.Message);
    }
}
