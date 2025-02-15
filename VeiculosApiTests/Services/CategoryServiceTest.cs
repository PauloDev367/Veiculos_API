using System;
using Microsoft.EntityFrameworkCore;
using VeiculosApi.Data;
using VeiculosApi.Exceptions;
using VeiculosApi.Http.Request;
using VeiculosApi.Models;
using VeiculosApi.Services;

namespace VeiculosApiTests.Services;

public class CategoryServiceTest
{
    private AppDbContext _memoryDbContext;
    private CategoryService _service;

    public CategoryServiceTest()
    {
        var dbContext = CreateInMemoryDbContext();
        _memoryDbContext = dbContext;
        _service = new CategoryService(_memoryDbContext);
    }

    private AppDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        return new AppDbContext(options);
    }
    [Fact]
    public async Task ShouldCreateANewCategoryIfAllDataIsCorrect()
    {
        var request = new CreateCategoryRequest { Name = "Test Category @asdajksl123" };

        var category = await _service.CreateAsync(request);

        Assert.NotNull(category);
        Assert.Equal("Test Category @asdajksl123", category.Name);
    }

    [Fact]
    public async Task ShouldNotCreateANewCategoryIfNameIsNotPassed()
    {
        var request = new CreateCategoryRequest { Name = "" };

        var exception = await Assert.ThrowsAsync<EmptyValueException>(() => _service.CreateAsync(request));
        var expectedMessage = "You need to informate the name to continue";
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public async Task ShouldGetOneCategoryIfItExists()
    {
        var request = new CreateCategoryRequest { Name = "Test Category" };
        var category = await _service.CreateAsync(request);

        var retrievedCategory = await _service.GetOneAsync(category.Id);

        Assert.NotNull(retrievedCategory);
        Assert.Equal(category.Id, retrievedCategory.Id);
    }
    [Fact]
    public async Task ShouldNotGetOneCategoryIfItNotExists()
    {
        var retrievedCategory = await _service.GetOneAsync(Guid.NewGuid());
        Assert.Null(retrievedCategory);
    }


    [Fact]
    public async Task ShouldGetAllCategories()
    {
        var request = new CreateCategoryRequest { Name = "Test Category" };
        await _service.CreateAsync(request);

        var categories = await _service.GetAllAsync(1, 1);
        Assert.Equal(categories.TotalRecords, 1);
    }

}
