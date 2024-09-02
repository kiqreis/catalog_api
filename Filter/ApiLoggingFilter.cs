using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Filter;

public class ApiLoggingFilter : IActionFilter
{
  private readonly ILogger<ApiLoggingFilter> _logger;

  public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
  {
    _logger = logger;
  }

  public void OnActionExecuting(ActionExecutingContext context)
  {
    _logger.LogInformation("### Running => OnActionExecuting ###");
    _logger.LogInformation("####################################");
    _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
    _logger.LogInformation($"ModelState: {context.ModelState.IsValid}");
    _logger.LogInformation("####################################");
  }

  public void OnActionExecuted(ActionExecutedContext context)
  {
    _logger.LogInformation("### Running => OnActionExecuted ###");
    _logger.LogInformation("####################################");
    _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
    _logger.LogInformation($"StatusCode: {context.HttpContext.Response.StatusCode}");
    _logger.LogInformation("####################################");
  }
}