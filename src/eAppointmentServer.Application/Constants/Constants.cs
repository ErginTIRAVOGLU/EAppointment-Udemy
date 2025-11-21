using System;
using eAppointmentServer.Domain.Entities.AppRole;

namespace eAppointmentServer.Application.Constants;

public static class Constants
{
    public static List<AppRole> GetRoles()
    {
        List<string> roles = new()
        {
            "Admin",
            "Doctor",
            "Personel"
        };
        return roles.Select(role => new AppRole
        {
            Name = role
        }).ToList();
    }
}

 