using AutoMapper;

using CP.Api.Context;
using CP.Api.DTOs;
using CP.Api.Models;

namespace CP.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public CategoryService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    //get all categories
    public ICollection<Category> GetAllCategories() => _dbContext.Categories.ToList();

    //get category by id
    public Category GetCategoryById(int id)
    {
        var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
        if (category == null)
        {
            throw new Exception("Category not found");
        }
        return category;
    }

    //create category
    public Category CreateCategory(CategoryDTO categoryDto)
    {
        var existCategory = _dbContext.Categories.FirstOrDefault(x => x.Name == categoryDto.Name);
        if (existCategory != null)
        {
            throw new Exception("Category already exists");
        }
        existCategory = _mapper.Map<Category>(categoryDto);
        _dbContext.Categories.Add(existCategory);
        _dbContext.SaveChanges();
        return existCategory;
    }

}

public interface ICategoryService
{
    ICollection<Category> GetAllCategories();
    Category GetCategoryById(int id);
    Category CreateCategory(CategoryDTO category);
}