using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using eAppointmentServer.Application.Services;
using eAppointmentServer.Domain.Entities.AppUser;
using Microsoft.IdentityModel.Tokens;

namespace eAppointmentServer.Infrastructure.Services;

internal sealed class JwtProvider : IJwtProvider
{
    public string GenerateToken(AppUser user)
    {
        List<Claim> claims = new()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("UserName", user.UserName!)
        };

        DateTime expires = DateTime.UtcNow.AddDays(1);

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes("ThisIsASecretThisIsASecretThisIsASecretThisIsASecretThisIsASecret"));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha512);

        JwtSecurityToken jwtSecurityToken = new(
           issuer: "eAppointmentServer",
           audience: "eAppointmentClients",
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
