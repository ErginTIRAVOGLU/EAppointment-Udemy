using eAppointmentServer.Application.Features.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TS.MediatR;

namespace eAppointmentServer.WebAPI.Controllers;

[Route("api/auth")]
[AllowAnonymous]
public sealed class AuthController(ISender sender) : BaseApiController(sender)
{
    [HttpPost]
    [Route("login")] 
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }
}

