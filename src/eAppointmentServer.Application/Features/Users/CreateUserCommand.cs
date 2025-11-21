using System;
using eAppointmentServer.Domain.Entities.AppRole;
using eAppointmentServer.Domain.Entities.AppUser;
using eAppointmentServer.Domain.Entities.Shared.ValueObjects;
using ErginWebDev.Result;
using Microsoft.AspNetCore.Identity;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Users;

public sealed record CreateUserCommand(
    string UserName,
    string Email,
    string FirstName,
    string LastName,
    string Password,
    List<Guid> RoleIds
): IRequest<Result<string>>;

internal sealed class CreateUserCommandHandler(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager
)
    : IRequestHandler<CreateUserCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new AppUser(new FirstName(request.FirstName), new LastName(request.LastName));

        user.UserName = request.UserName;
        user.Email = request.Email; 
        

        var result = await userManager.CreateAsync(user, request.Password);
        
        if (result.Succeeded)
        {
            // Get role names from role IDs
            var roles = new List<string>();
            foreach (var roleId in request.RoleIds)
            {
                var role = await roleManager.FindByIdAsync(roleId.ToString());
                if (role != null)
                {
                    roles.Add(role.Name!);
                }
            }

            // Add user to roles
            if (roles.Any())
            {
                var roleResult = await userManager.AddToRolesAsync(user, roles);
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    return Result<string>.Fail($"User created but failed to assign roles: {errors}");
                }
            }
            
            return Result<string>.Success("User created successfully.");
        }
        else
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<string>.Fail(errors);
        }
    }
}