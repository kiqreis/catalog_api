using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[Route("api/v{version:apiVersion}/test")]
[ApiController]
[ApiVersion("2.0")]
public class TestV2Controller : ControllerBase
{
  [HttpGet]
  public string GetVersion()
  {
    return "This is the second api version (2.0)";
  }
}