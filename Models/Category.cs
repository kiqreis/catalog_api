using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(80)]
    public string? Name { get; set; }
    
    public ICollection<Product>? Products { get; set; }
}   