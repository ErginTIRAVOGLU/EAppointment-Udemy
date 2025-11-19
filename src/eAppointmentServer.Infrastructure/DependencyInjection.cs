using eAppointmentServer.Application.Services;
using eAppointmentServer.Domain.Entities.AppRole;
using eAppointmentServer.Domain.Entities.AppUser;
using eAppointmentServer.Domain.Repositories;
using eAppointmentServer.Infrastructure.Context;
using eAppointmentServer.Infrastructure.Repositories;
using eAppointmentServer.Infrastructure.Services;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eAppointmentServer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDI(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddIdentity<AppUser, AppRole>(action =>
        {
            action.Password.RequireDigit = true;
            action.Password.RequireLowercase = true;
            action.Password.RequireUppercase = true;
            action.Password.RequireNonAlphanumeric = false;
            action.Password.RequiredLength = 6;
        }).AddEntityFrameworkStores<ApplicationDbContext>();
        
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IJwtProvider, JwtProvider>();
        
        return services;
    }
}
