using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeiculosApi.Http.Request;
using VeiculosApi.Http.Response;
using VeiculosApi.Models;
using VeiculosApi.Services;

namespace VeiculosApi.Controllers;

[Route("api/v1/categories")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _service;
    public CategoryController(CategoryService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryRequest request)
    {
        try
        {
            var created = await _service.CreateAsync(request);
            return Ok(new DefaultControllerResponse<Category>
            {
                Status = 200,
                Message = "Category created successfullys",
                Data = created,
            });

        }
        catch (DbUpdateException ex)
        {
            return BadRequest(new DefaultControllerResponse<string>
            {
                Status = 400,
                Message = "Category was already created"
            }
            );
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOneAsync(Guid id)
    {
        var category = await _service.GetOneAsync(id);
        if (category == null)
        {
            return NotFound(new DefaultControllerResponse<string>
            {
                Status = 400,
                Message = "Category was not foundend"
            }
            );
        }

        return Ok(new DefaultControllerResponse<Category>
        {
            Status = 200,
            Message = "Category was foundend",
            Data = category
        }
        );
    }
}
