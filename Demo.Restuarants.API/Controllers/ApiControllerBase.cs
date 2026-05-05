using Microsoft.AspNetCore.Mvc;

namespace Demo.Restuarants.API.Controllers;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class ApiControllerBase<TDerived>
(
    ILogger<TDerived> logger
) : ControllerBase
{
    protected ILogger<TDerived> Logger { get; init; } = logger;
}