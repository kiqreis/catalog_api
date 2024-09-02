using System.ComponentModel.DataAnnotations;
using AutoMapper;
using WebApplication1.Validations;

namespace WebApplication1.Dtos;

public class ProductDTO 
{
  public int Id { get; set; }

  [Required]
  [StringLength(255, MinimumLength = 3)]
  [FirstLetterCapitalized]
  public string? Name { get; set; }

  [Required] 
  [StringLength(255)] public string? Description { get; set; }

  public decimal Price { get; set; }

  [Required] 
  [StringLength(255)] 
  public string? ImgUrl { get; set; }

  [Required] 
  [Range(1, int.MaxValue)] 
  public int Stock { get; set; }
  public DateTime RegistrationDate { get; set; }

  public int CategoryId { get; set; }
}