using System;
using eAppointmentServer.Domain.Entities.AppUser;
using ErginWebDev.Result;
using Microsoft.AspNetCore.Identity;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Users;

public sealed record GetAllUsersQuery(): IRequest<Result<List<GetAllUsersDto>>>;

internal sealed class GetAllUsersQueryHandler(
    UserManager<AppUser> userManager
)
    : IRequestHandler<GetAllUsersQuery, Result<List<GetAllUsersDto>>>
{
    public Task<Result<List<GetAllUsersDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = userManager.Users
            .Select(u => new GetAllUsersDto(
                u.Id,
                u.UserName!,
                u.Email!,
                u.FirstName.Value,
                u.LastName.Value
            ))
            .ToList();
            
        return Task.FromResult(Result<List<GetAllUsersDto>>.Success(users));
    }
}
