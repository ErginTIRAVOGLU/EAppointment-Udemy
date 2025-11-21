using eAppointmentServer.Domain.Entities.AppUser;

namespace eAppointmentServer.Application.Services;

public interface IJwtProvider
{
    Task<string> GenerateToken(AppUser user);
}
