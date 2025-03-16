using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ShiftsLogger.Api.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    [Route("/error")]
    public IActionResult Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        var statusCode = 500;

        if (exception != null)
        {
            _logger.LogError(exception, "Unexpected error: {Message}", exception.Message);
        }

        return Problem(
            title: "An error occurred while processing your request.",
            statusCode: statusCode
        );
    }
} 