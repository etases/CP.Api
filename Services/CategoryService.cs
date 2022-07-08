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
    public ICollection<CategoryOutput> GetAllCategories()
    {
        List<Category> categories = _dbContext.Categories.ToList();
        ICollection<CategoryOutput>? output = _mapper.Map<ICollection<CategoryOutput>>(categories);
        return output;
    }

    //get category by id
    public CategoryOutput? GetCategoryById(int id)
    {
        Category? category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
        if (category == null)
        {
            return null;
        }

        CategoryOutput? output = _mapper.Map<CategoryOutput>(category);
        return output;
    }

    //create category
    public CategoryOutput? CreateCategory(CategoryInput categoryInput)
    {
        Category? existCategory = _dbContext.Categories.FirstOrDefault(x => x.Name == categoryInput.Name);
        if (existCategory != null)
        {
            return null;
        }

        existCategory = _mapper.Map<Category>(categoryInput);
        _dbContext.Categories.Add(existCategory);
        _dbContext.SaveChanges();
        CategoryOutput? output = _mapper.Map<CategoryOutput>(existCategory);
        return output;
    }
}

public interface ICategoryService
{
    ICollection<CategoryOutput> GetAllCategories();
    CategoryOutput? GetCategoryById(int id);
    CategoryOutput? CreateCategory(CategoryInput category);
}