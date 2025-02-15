using System;
using Microsoft.IdentityModel.Tokens;
using VeiculosApi.Data;
using VeiculosApi.Exceptions;
using VeiculosApi.Http.Request;
using VeiculosApi.Models;

namespace VeiculosApi.Services;

public class CategoryService
{
    private readonly AppDbContext _context;
    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> CreateAsync(CreateCategoryRequest request)
    {
        if(request.Name.IsNullOrEmpty()){
            throw new EmptyValueException("You need to informate the name to continue");
        }

        var category = new Category
        {
            Name = request.Name
        };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }
}
