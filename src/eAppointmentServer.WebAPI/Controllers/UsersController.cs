using System;
using eAppointmentServer.Application.Features.Users;
using Microsoft.AspNetCore.Mvc;
using TS.MediatR;

namespace eAppointmentServer.WebAPI.Controllers;

[Route("api/users")]
public class UsersController(ISender sender) : BaseApiController(sender)
{
    [HttpGet]
    [Route("")] // GET /api/users
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var query = new GetAllUsersQuery();
        var result = await Sender.Send(query, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet]
    [Route("{userId:guid}/roles")] // GET /api/users/{userId}/roles
    public async Task<IActionResult> GetUserRoles([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var query = new GetUserRolesQuery(userId);
        var result = await Sender.Send(query, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpDelete]
    [Route("{userId:guid}")] // DELETE /api/users/{userId}
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(userId);
        var result = await Sender.Send(command, cancellationToken);
        return StatusCode((int)result.StatusCode, result);  
    }

    [HttpPost]
    [Route("")] // POST /api/users
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }
    [HttpPut]
    [Route("{id:guid}")] // PUT /api/users/{id}
    public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        
        var result = await Sender.Send(command, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }
}
