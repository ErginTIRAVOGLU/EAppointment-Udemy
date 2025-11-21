using System;
using eAppointmentServer.Domain.Entities.AppRole;
using ErginWebDev.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Roles;

public sealed record RoleSyncCommand() : IRequest<Result<string>>;

internal sealed class RoleSyncCommandHandler(
    RoleManager<AppRole> roleManager
) 
    : IRequestHandler<RoleSyncCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleSyncCommand request, CancellationToken cancellationToken)
    {
        List<AppRole> currentRoles = await roleManager.Roles.ToListAsync(cancellationToken);
        List<AppRole> staticRoles = Constants.Constants.GetRoles();

        // Static rolleri dolaş, mevcut değilse oluştur
        foreach (var staticRole in staticRoles)
        {
            if (!currentRoles.Any(r => r.Name == staticRole.Name))
            {
                await roleManager.CreateAsync(staticRole);
            }
        }

        // Static'de olmayan mevcut rolleri sil
        foreach (var currentRole in currentRoles)
        {
            if (!staticRoles.Any(r => r.Name == currentRole.Name))
            {
                await roleManager.DeleteAsync(currentRole);
            }
        }

        return Result<string>.Success("Roles synchronized successfully.");
         
    }
}