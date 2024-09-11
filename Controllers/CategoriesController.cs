using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;

[Route("[controller]")]
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
public class CategoriesController(IUnitOfWork unitOf, IMapper mapper) : ControllerBase
{
  [HttpGet("pagination")]
  public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get([FromQuery] CategoriesParameters categoriesParams)
  {
    var categories = await unitOf.CategoryRepository.GetCategoriesAsync(categoriesParams);
    var categoriesDto = mapper.Map<IEnumerable<CategoryDTO>>(categories);
    
    var metadata = new
    {
      categories.Count,
      categories.PageSize,
      categories.PageCount,
      categories.TotalItemCount,
      categories.HasNextPage,
      categories.HasPreviousPage
    };
    
    Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

    return Ok(categoriesDto);
  }
  
  [HttpGet]
  public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
  {
    var categories = await unitOf.CategoryRepository.GetAllAsync();
    var categoriesDto = mapper.Map<IEnumerable<CategoryDTO>>(categories);
    
    return !categoriesDto.Any() ? NotFound("No categories found") : Ok(categoriesDto);
  }

  [HttpGet("{id:int}", Name = "GetCategory")]
  public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
  {
    var category = await unitOf.CategoryRepository.GetAsync(c => c.Id == id);

    if (category == null)
    {
      return NotFound("Category not found");
    }
    
    var categoryDto = mapper.Map<CategoryDTO>(category);
    
    return Ok(categoryDto);
  }

  [HttpPost]
  public async Task<ActionResult<CategoryDTO>> PostCategory(CategoryDTO categoryDto)
  {
    var category = mapper.Map<Category>(categoryDto);
    
    category = unitOf.CategoryRepository.Create(category);
    await unitOf.CommitAsync();

    var newCategoryDto = mapper.Map<CategoryDTO>(category);
    
    return new CreatedAtRouteResult("GetCategory", new { id = newCategoryDto.Id }, newCategoryDto);
  }

  [HttpPut("{id:int}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<CategoryDTO>> PutCategory(int id, CategoryDTO categoryDto)
  {
    if (id != categoryDto.Id) return BadRequest("Inconsistent data");

    var category = mapper.Map<Category>(categoryDto); 
    
    unitOf.CategoryRepository.Update(category);
    await unitOf.CommitAsync();
    
    return Ok(categoryDto);
  }

  [HttpDelete("{id:int}")]
  public async Task<ActionResult> DeleteCategory(int id)
  {
    unitOf.CategoryRepository.Delete(id);
    await unitOf.CommitAsync();
    
    return NoContent();
  }
}
