using System;
using eAppointmentServer.Application.Features.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TS.MediatR;

namespace eAppointmentServer.WebAPI.Controllers;


[Route("api/roles")]
public sealed class RolesController(ISender sender) : BaseApiController(sender)
{
    [HttpPost]
    [AllowAnonymous]
    [Route("sync")] // POST /api/roles/sync
    public async Task<IActionResult> SyncRoles(RoleSyncCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet]
    [Route("")] // GET /api/roles
    public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
    {
        var query = new GetAllRolesQuery();
        var result = await Sender.Send(query, cancellationToken);
        return StatusCode((int)result.StatusCode, result);  
    }
}
