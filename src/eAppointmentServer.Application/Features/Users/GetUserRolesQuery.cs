using System;
using ErginWebDev.Result;
using Microsoft.AspNetCore.Identity;
using TS.MediatR;
using eAppointmentServer.Domain.Entities.AppUser;
using eAppointmentServer.Domain.Entities.AppRole;

namespace eAppointmentServer.Application.Features.Users;

public sealed record GetUserRolesQuery(
    Guid UserId
) : IRequest<Result<List<Guid>>>;

internal sealed class GetUserRolesQueryHandler(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager
) : IRequestHandler<GetUserRolesQuery, Result<List<Guid>>>
{
    public async Task<Result<List<Guid>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            return Result<List<Guid>>.Fail("User not found.");
        }

        var userRoles = await userManager.GetRolesAsync(user);
        var roleIds = new List<Guid>();

        foreach (var roleName in userRoles)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                roleIds.Add(role.Id);
            }
        }

        return Result<List<Guid>>.Success(roleIds);
    }
}