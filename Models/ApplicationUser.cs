using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models;

public class ApplicationUser : IdentityUser
{
  public string? RefreshToken { get; set; }
  public DateTime RefreshTokenExpiryTime { get; set; }
}