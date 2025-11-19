using eAppointmentServer.Domain.Entities.AppUser;
using eAppointmentServer.Domain.Entities.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace eAppointmentServer.WebAPI;

public static class Helper
{
    public static async Task CreateUserAsync(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            if (!userManager.Users.Any())
            {
                await userManager.CreateAsync(new AppUser(
                    firstName: new FirstName("Admin"),
                    lastName: new LastName("User")
                )
                {
                    UserName = "admin",
                    Email = "admin@admin.com"
                }, "Password100$");
            }
        }
    }
}
