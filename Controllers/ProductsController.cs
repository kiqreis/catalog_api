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
public class ProductsController(IUnitOfWork unitOf, IMapper mapper) : ControllerBase
{
  [HttpGet("pagination")]
  public async Task<ActionResult<IEnumerable<ProductDTO>>> Get([FromQuery] ProductsParameters productsParams)
  {
    var products = await unitOf.ProductRepository.GetProductsAsync(productsParams);
    var productsDto = mapper.Map<IEnumerable<ProductDTO>>(products);

    var metadata = new
    {
      products.Count,
      products.PageSize,
      products.PageCount,
      products.TotalItemCount,
      products.HasNextPage,
      products.HasPreviousPage
    };

    Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
    
    return Ok(productsDto);
  }

  [HttpGet] 
  public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
  {
    var products = await unitOf.ProductRepository.GetAllAsync();
    var productsDto = mapper.Map<IEnumerable<ProductDTO>>(products);

    return !productsDto.Any() ? NotFound("No products found") : Ok(productsDto);
  }

  [HttpGet("{id:int}", Name = "GetProduct")]
  public async Task<ActionResult<ProductDTO>> GetProduct(int id)
  {
    var product = await unitOf.ProductRepository.GetAsync(p => p.Id == id);
    
    if (product == null)
    {
      return NotFound("Product not found!");
    }
    
    var productDto = mapper.Map<ProductDTO>(product);

    return Ok(productDto);
  }

  [HttpPost]
  public async Task<ActionResult<ProductDTO>> PostProduct(ProductDTO productDto)
  {
    var product = mapper.Map<Product>(productDto);

    product = unitOf.ProductRepository.Create(product);
    await unitOf.CommitAsync();

    var newProductDto = mapper.Map<ProductDTO>(product);
    
    return new CreatedAtRouteResult("GetProduct", new { id = newProductDto.Id }, newProductDto);
  }

  [HttpPut("{id:int}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<ProductDTO>> PutProduct(int id, ProductDTO productDto)
  {
    if (id != productDto.Id) return BadRequest("Inconsistent data");

    var product = mapper.Map<Product>(productDto);

    unitOf.ProductRepository.Update(product);
    await unitOf.CommitAsync();

    return Ok(productDto);
  }

  [HttpDelete("{id:int}")]
  public async Task<ActionResult> DeleteProduct(int id)
  {
    unitOf.ProductRepository.Delete(id);
    await unitOf.CommitAsync();

    return NoContent();
  }
}