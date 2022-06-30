using CP.Api.DTOs;
using CP.Api.DTOs.Response;
using CP.Api.Models;
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
    public ICollection<Category> GetAllCategories() => _categoryService.GetAllCategories();

    //get category by id
    [HttpGet("{id}")]
    public Category GetCategoryById(int id) => _categoryService.GetCategoryById(id);

    //create category
    [HttpPost]
    public ActionResult<Category> CreateCategory(CategoryDTO categoryDto)
    {
        var newCategory = _categoryService.CreateCategory(categoryDto);
        return CreatedAtAction(nameof(GetCategoryById), new { id = newCategory.Id }, newCategory);
    }
}