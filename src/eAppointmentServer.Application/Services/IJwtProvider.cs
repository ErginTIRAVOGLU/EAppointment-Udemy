using System;
using eAppointmentServer.Domain.Entities.AppUser;

namespace eAppointmentServer.Application.Services;

public interface IJwtProvider
{
    string GenerateToken(AppUser user);
}
