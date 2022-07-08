using CP.Api.DTOs;
using CP.Api.DTOs.Response;
using CP.Api.Models;
using CP.Api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CP.Api.Controllers;

/// <summary>
///     Category API controller
/// </summary>
[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    /// <summary>
    ///     Category controller constructor
    /// </summary>
    /// <param name="categoryService">Category service</param>
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    ///     Get all categories
    /// </summary>
    /// <returns>ResponseDTO <seealso cref="CategoryOutput[]" /></returns>
    [HttpGet]
    public ResponseDTO<ICollection<CategoryOutput>> GetAllCategories()
    {
        return new ResponseDTO<ICollection<CategoryOutput>>
        {
            Data = _categoryService.GetAllCategories(), Success = true, Message = "Get all categories"
        };
    }

    /// <summary>
    ///     Get category by id
    /// </summary>
    /// <param name="id">Id of the category</param>
    /// <returns>ResponseDTO <seealso cref="CategoryOutput" /></returns>
    [HttpGet("{id}")]
    public ActionResult<ResponseDTO<CategoryOutput>> GetCategoryById(int id)
    {
        CategoryOutput? category = _categoryService.GetCategoryById(id);
        return category switch
        {
            null => NotFound(new ResponseDTO<CategoryOutput> {Message = "Category not found", Success = false}),
            _ => Ok(new ResponseDTO<CategoryOutput> {Data = category, Success = true, Message = "Category found"})
        };
    }

    /// <summary>
    ///     Create category
    /// </summary>
    /// <param name="categoryInput">Information to create category</param>
    /// <returns>ResponseDTO <seealso cref="CategoryOutput" /></returns>
    [HttpPost]
    [Authorize(Roles = DefaultRoles.AdministratorString)]
    public ActionResult<ResponseDTO<CategoryOutput>> CreateCategory(CategoryInput categoryInput)
    {
        CategoryOutput? category = _categoryService.CreateCategory(categoryInput);
        return category switch
        {
            null => Conflict(new ResponseDTO<CategoryOutput> {Message = "Category existed", Success = false}),
            _ => Ok(new ResponseDTO<CategoryOutput> {Data = category, Success = true, Message = "Category created"})
        };
    }
}