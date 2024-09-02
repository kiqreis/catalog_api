using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace WebApplication1.Dtos;

public class CategoryDTO : Profile
{
  public int Id { get; set; }
    
  [Required]
  [StringLength(80)]
  public string? Name { get; set; }
}