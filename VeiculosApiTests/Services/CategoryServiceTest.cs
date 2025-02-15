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
        var dbContext = CreateInMemoryDbContext();
        var service = new CategoryService(dbContext);
        var request = new CreateCategoryRequest { Name = "Test Category @asdajksl123" };

        var category = await service.CreateAsync(request);

        Assert.NotNull(category);
        Assert.Equal("Test Category @asdajksl123", category.Name);

        var savedCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == "Test Category @asdajksl123");
        Assert.NotNull(savedCategory);
    }

    [Fact]
    public async Task ShouldNotCreateANewCategoryIfNameIsNotPassed()
    {
        var dbContext = CreateInMemoryDbContext();
        var service = new CategoryService(dbContext);
        var request = new CreateCategoryRequest { Name = "" };

        var exception = await Assert.ThrowsAsync<EmptyValueException>(() => service.CreateAsync(request));
        var expectedMessage = "You need to informate the name to continue";
        Assert.Equal(expectedMessage, exception.Message);
    }
}
