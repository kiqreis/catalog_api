using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[Route("api/v{version:apiVersion}/test")]
[ApiController]
[ApiVersion(3)]
[ApiVersion(4)]
public class TestV3Controller : ControllerBase
{
  [HttpGet]
  [MapToApiVersion(3)]
  public string GetVersion3()
  {
    return "This is the third api version (3.0)";
  }
  
  [HttpGet]
  [MapToApiVersion(4)]
  public string GetVersion4()
  {
    return "This is the fourth api version (4.0)";
  }
}