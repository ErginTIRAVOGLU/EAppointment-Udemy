using System;
using eAppointmentServer.Domain.Entities.AppUser;
using ErginWebDev.Result;
using Microsoft.AspNetCore.Identity;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Users;

public sealed record DeleteUserCommand(Guid UserId): IRequest<Result<string>>;
internal sealed class DeleteUserCommandHandler(
    UserManager<AppUser> userManager
)
    : IRequestHandler<DeleteUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            return Result<string>.Fail("User not found.");
        }
        
        var result = await userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return Result<string>.Success("User deleted successfully.");
        }
        else
        {
            return Result<string>.Fail("Failed to delete user.");
        }
    }
}
