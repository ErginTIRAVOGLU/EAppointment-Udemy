using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using eAppointmentServer.Application.Services;
using eAppointmentServer.Domain.Entities.AppUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace eAppointmentServer.Infrastructure.Services;

internal sealed class JwtProvider(
    IConfiguration configuration,
    UserManager<AppUser> userManager) : IJwtProvider
{
    public async Task<string> GenerateToken(AppUser user)
    {
        List<string> userRoles = (await userManager.GetRolesAsync(user)).ToList();

        List<Claim> claims = new()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("UserName", user.UserName!),
            new Claim(ClaimTypes.Role, JsonSerializer.Serialize(userRoles))
        };

        DateTime expires = DateTime.UtcNow.AddDays(1);

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha512);

        JwtSecurityToken jwtSecurityToken = new(
           issuer: configuration["Jwt:Issuer"],
           audience: configuration["Jwt:Audience"],
           claims: claims,
           notBefore: DateTime.UtcNow,
           expires: expires,
           signingCredentials: credentials
       );

        JwtSecurityTokenHandler tokenHandler = new();
        string token = tokenHandler.WriteToken(jwtSecurityToken);
        return token;
    }
}
