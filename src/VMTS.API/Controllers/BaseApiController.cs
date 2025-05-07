using Microsoft.AspNetCore.Mvc;

namespace VMTS.API.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
    protected HttpValidationProblemDetails ValidationProblemDetails(
        IDictionary<string, string[]> errors
    )
    {
        return new HttpValidationProblemDetails(errors)
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred.",
        };
    }
}
