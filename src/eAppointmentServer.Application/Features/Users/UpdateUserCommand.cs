using System;
using eAppointmentServer.Domain.Entities.AppRole;
using eAppointmentServer.Domain.Entities.AppUser;
using eAppointmentServer.Domain.Entities.Shared.ValueObjects;
using ErginWebDev.Result;
using Microsoft.AspNetCore.Identity;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Users;

public sealed record UpdateUserCommand
(
    Guid Id,
    string UserName,
    string Email,
    string FirstName,
    string LastName,
    List<Guid> RoleIds
) : IRequest<Result<string>>;   
 

 internal sealed class UpdateUserCommandHandler(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager
) : IRequestHandler<UpdateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user =  await userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            return Result<string>.Fail("User not found.");
        }

        user.UserName = request.UserName;
        user.Email = request.Email;
        user.SetFirstName(new FirstName(request.FirstName));
        user.SetLastName(new LastName(request.LastName));
        

        var result = await userManager.UpdateAsync(user);

        // Update user roles
        if (result.Succeeded)
        {
            // Get current user roles
            var currentRoles = await userManager.GetRolesAsync(user);
            
            // Get new role names from role IDs
            var newRoles = new List<string>();
            foreach (var roleId in request.RoleIds)
            {
                var role = await roleManager.FindByIdAsync(roleId.ToString());
                if (role != null)
                {
                    newRoles.Add(role.Name!);
                }
            }

            // Remove roles that are not in the new list
            var rolesToRemove = currentRoles.Except(newRoles).ToList();
            if (rolesToRemove.Any())
            {
                var removeResult = await userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                {
                    var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                    return Result<string>.Fail($"Failed to remove old roles: {errors}");
                }
            }

            // Add new roles
            var rolesToAdd = newRoles.Except(currentRoles).ToList();
            if (rolesToAdd.Any())
            {
                var addResult = await userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded)
                {
                    var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
                    return Result<string>.Fail($"Failed to add new roles: {errors}");
                }
            }
        }

        if (result.Succeeded)
        {
            return Result<string>.Success("User updated successfully.");
        }
        else
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<string>.Fail(errors);
        }
    }
}