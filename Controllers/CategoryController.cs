using CP.Api.DTOs;
using CP.Api.DTOs.Response;
using CP.Api.Extensions;
using CP.Api.Services;

using Microsoft.AspNetCore.Mvc;

namespace CP.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    //get all categories
    [HttpGet]
    public PaginationResponseDTO<CategoryOutput> GetAllCategories(PaginationParameter parameter)
    {
        var pagedOutput = _categoryService.GetAllCategories()
            .GetCount(out var count)
            .GetPage(parameter);
        return new PaginationResponseDTO<CategoryOutput>
        {
            Data = pagedOutput, Success = true, Message = "Get all categories", TotalRecords = count
        };
    }

    //get category by id
    [HttpGet("{id}")]
    public ActionResult<ResponseDTO<CategoryOutput>> GetCategoryById(int id)
    {
        CategoryOutput? category = _categoryService.GetCategoryById(id);
        if (category == null)
        {
            return NotFound(new ResponseDTO {Success = false, Message = "Category not found"});
        }

        return Ok(new ResponseDTO<CategoryOutput> {Data = category, Success = true, Message = "Get category by id"});
    }

    //create category
    [HttpPost]
    public ActionResult<ResponseDTO<CategoryOutput>> CreateCategory(CategoryInput categoryInput)
    {
        CategoryOutput? category = _categoryService.CreateCategory(categoryInput);
        if (category == null)
        {
            return BadRequest(new ResponseDTO {Success = false, Message = "Category existed"});
        }

        return CreatedAtAction(nameof(GetCategoryById), new {id = category.Id},
            new ResponseDTO<CategoryOutput> {Data = category, Success = true, Message = "Category created"});
    }
}