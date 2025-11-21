using System;
using eAppointmentServer.Domain.Entities.AppRole;
using ErginWebDev.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Roles;

public sealed record GetAllRolesQuery() : IRequest<Result<List<GetAllRolesDto>>>;
internal sealed class GetAllRolesQueryHandler(
    RoleManager<AppRole> roleManager
) : IRequestHandler<GetAllRolesQuery, Result<List<GetAllRolesDto>>>
{
    public async Task<Result<List<GetAllRolesDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        List<AppRole> roleList= await roleManager
            .Roles
            .ToListAsync(cancellationToken);
        
        // Map to DTOs in memory (client-side)
        List<GetAllRolesDto> roles = roleList
            .OrderBy(r => r.Name)
            .Select(r => new GetAllRolesDto(
                r.Id,
                r.Name!
            ))
            .ToList();

        return Result<List<GetAllRolesDto>>.Success(roles);
    }
}