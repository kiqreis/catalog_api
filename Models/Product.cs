using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApplication1.Validations;

namespace WebApplication1.Models;

public class Product
{
  [Key] 
  public int Id { get; set; }

  [Required]
  [StringLength(255, MinimumLength = 3)]
  [FirstLetterCapitalized]
  public string? Name { get; set; }

  [Required] 
  [StringLength(255)] 
  public string? Description { get; set; }

  [Column(TypeName = ("decimal(11, 2)"))]
  public decimal Price { get; set; }

  [Required] 
  [StringLength(255)] 
  public string? ImgUrl { get; set; }

  [Required] 
  [Range(1, int.MaxValue)]
  public int Stock { get; set; }
  public DateTime RegistrationDate { get; set; }

  public int CategoryId { get; set; }

  [JsonIgnore] 
  public Category? Category { get; set; }
}