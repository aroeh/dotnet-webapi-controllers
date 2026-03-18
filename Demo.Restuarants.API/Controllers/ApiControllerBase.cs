using Microsoft.AspNetCore.Mvc;

namespace Demo.Restuarants.API.Controllers;

[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
public class ApiControllerBase<TDerived>
(
    ILogger<TDerived> logger
) : ControllerBase
{
    protected ILogger<TDerived> Logger { get; init; } = logger;
}