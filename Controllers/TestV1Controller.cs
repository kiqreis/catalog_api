using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[Route("api/v{version:apiVersion}/test/")]
[ApiController]
[ApiVersion("1.0")]
public class TestV1Controller : ControllerBase
{
  [HttpGet]
  public string GetVersion()
  {
    return "This is the first api version (1.0)";
  }
}