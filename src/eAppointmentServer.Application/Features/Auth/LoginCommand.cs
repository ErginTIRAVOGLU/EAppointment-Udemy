using System.Net;
using eAppointmentServer.Application.Services;
using eAppointmentServer.Domain.Entities.AppUser;
using ErginWebDev.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Auth;

public sealed record LoginCommand(
    string UsernameOrEmail,
    string Password) : IRequest<Result<LoginCommandResponse>>;

public sealed record LoginCommandResponse(
    string Token);

internal sealed class LoginCommandHandler(
    UserManager<AppUser> userManager,
    IJwtProvider jwtProvider
) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.Users.FirstOrDefaultAsync(u =>
            u.UserName == request.UsernameOrEmail ||
            u.Email == request.UsernameOrEmail, cancellationToken);
        
        if (user is null)
        {
            return Result<LoginCommandResponse>.Fail("Invalid username or password.",statusCode : HttpStatusCode.Unauthorized);
        }

        bool isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            return Result<LoginCommandResponse>.Fail("Invalid username or password.",statusCode : HttpStatusCode.Unauthorized);
        }
        var token = jwtProvider.GenerateToken(user);
        return Result<LoginCommandResponse>.Success(new LoginCommandResponse(Token: token));
    }
}