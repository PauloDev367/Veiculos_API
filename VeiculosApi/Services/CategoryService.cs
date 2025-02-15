using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VeiculosApi.Data;
using VeiculosApi.Exceptions;
using VeiculosApi.Http.Request;
using VeiculosApi.Http.Response;
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
        if (request.Name.IsNullOrEmpty())
        {
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

    public async Task<Category?> GetOneAsync(Guid id)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<PageResultResponse<Category>> GetAllAsync(int pageNumber, int pageSize)
    {
        if (pageSize > 10) pageSize = 10;

        var totalRecords = await _context.Categories.CountAsync();
        var categories = await _context.Categories
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new PageResultResponse<Category>
        {
            TotalRecords = totalRecords,
            Data = categories,
            Page = pageNumber,
            PageSize = pageSize
        };

        return result;
    }

    public async Task DeleteAsync(Guid id)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
            throw new ModelNotFoundException("Category was not founded");

        _context.Remove(category);
        await _context.SaveChangesAsync();
    }
}
