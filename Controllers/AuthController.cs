using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[Route("[controller]")]
[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class AuthController(
  ITokenService tokenService,
  UserManager<ApplicationUser> userManager,
  RoleManager<IdentityRole> roleManager,
  IConfiguration configuration)
  : ControllerBase
{
  [HttpPost]
  [Route("login")]
  public async Task<IActionResult> Login([FromBody] LoginModel model)
  {
    var user = await userManager.FindByNameAsync(model.Username!);

    if (user is not null && await userManager.CheckPasswordAsync(user, model.Password!))
    {
      var userRoles = await userManager.GetRolesAsync(user);

      var authClaims = new List<Claim>
      {
        new(ClaimTypes.Name, user.UserName!),
        new(ClaimTypes.Email, user.Email!),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      };

      authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

      var token = tokenService.GenerateAccessToken(authClaims, configuration);
      var refreshToken = tokenService.GenerateRefreshToken();

      _ = int.TryParse(configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

      user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);
      user.RefreshToken = refreshToken;

      await userManager.UpdateAsync(user);

      return Ok(new
      {
        Token = new JwtSecurityTokenHandler().WriteToken(token),
        RefreshToken = refreshToken,
        Expiration = token.ValidTo
      });
    }

    return Unauthorized();
  }

  [HttpPost]
  [Route("register")]
  public async Task<IActionResult> Register([FromBody] RegisterModel model)
  {
    var userExists = await userManager.FindByNameAsync(model.Username!);

    if (userExists != null)
    {
      return StatusCode(StatusCodes.Status500InternalServerError,
        new Response { Status = "Error", Message = "User already exists" });
    }

    var user = new ApplicationUser
    {
      Email = model.Email,
      SecurityStamp = Guid.NewGuid().ToString(),
      UserName = model.Username
    };

    var result = await userManager.CreateAsync(user, model.Password!);

    if (!result.Succeeded)
    {
      return StatusCode(StatusCodes.Status500InternalServerError,
        new Response { Status = "Error", Message = "User creation failed" });
    }

    return Ok(new Response { Status = "Success", Message = "User created successfully" });
  }

  [HttpPost]
  [Route("refresh-token")]
  public async Task<IActionResult> RefreshToken(TokenModel? tokenModel)
  {
    if (tokenModel is null)
    {
      return BadRequest("Invalid client request");
    }

    var accessToken = tokenModel.AccessToken ?? throw new ArgumentNullException(nameof(tokenModel));
    var refreshToken = tokenModel.RefreshToken ?? throw new ArgumentException(nameof(tokenModel));
    var principal = tokenService.GetPrincipalFromExpiredToken(accessToken, configuration);
    var username = principal.Identity!.Name;
    var user = await userManager.FindByNameAsync(username!);

    if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
    {
      return BadRequest("Invalid access token/refresh token");
    }

    var newAccessToken = tokenService.GenerateAccessToken(principal.Claims.ToList(), configuration);
    var newRefreshToken = tokenService.GenerateRefreshToken();

    user.RefreshToken = newRefreshToken;
    await userManager.UpdateAsync(user);

    return new ObjectResult(new
    {
      accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
      refreshToken = newRefreshToken
    });
  }

  [Authorize]
  [HttpPost]
  [Route("revoke/{username}")]
  public async Task<IActionResult> Revoke(string username)
  {
    var user = await userManager.FindByNameAsync(username);

    if (user is null) return BadRequest("Invalid username");

    user.RefreshToken = null;

    await userManager.UpdateAsync(user);

    return NoContent();
  }

  [HttpPost]
  [Route("create-role")]
  public async Task<IActionResult> CreateRole(string roleName)
  {
    var roleExist = await roleManager.RoleExistsAsync(roleName);

    if (!roleExist)
    {
      var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));

      if (roleResult.Succeeded)
      {
        return StatusCode(StatusCodes.Status200OK,
          new Response { Status = "Success", Message = $"Role {roleName} added successfully" });
      }
    }
    else
    {
      return StatusCode(StatusCodes.Status400BadRequest,
        new Response { Status = "Error", Message = $"Issue adding the {roleName} role" });
    }

    return StatusCode(StatusCodes.Status400BadRequest,
      new Response { Status = "Error", Message = "Role already exists" });
  }

  [HttpPost]
  [Route("add-user-to-role")]
  public async Task<IActionResult> AddUserToRole(string email, string roleName)
  {
    var user = await userManager.FindByEmailAsync(email);

    if (user != null)
    {
      var result = await userManager.AddToRoleAsync(user, roleName);

      if (result.Succeeded)
      {
        return StatusCode(StatusCodes.Status200OK,
          new Response { Status = "Success", Message = $"User {user} added to the {roleName} role" });
      }
      else
      {
        return StatusCode(StatusCodes.Status400BadRequest,
          new Response { Status = "Error", Message = $"Unable to add user {user} to the {roleName} role" });
      }
    }

    return StatusCode(StatusCodes.Status400BadRequest,
      new Response { Status = "Error", Message = "Unable to find user" });
  }
}