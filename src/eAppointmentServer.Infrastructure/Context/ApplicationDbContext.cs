using System.Reflection;
using eAppointmentServer.Domain.Entities.Appointment;
using eAppointmentServer.Domain.Entities.AppRole;
using eAppointmentServer.Domain.Entities.AppUser;
using eAppointmentServer.Domain.Entities.Doctor;
using eAppointmentServer.Domain.Entities.Patient;
using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eAppointmentServer.Infrastructure.Context;

internal sealed class ApplicationDbContext : IdentityDbContext<AppUser,AppRole,Guid>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Doctor> Doctors  { get; set; }
    public DbSet<Patient> Patients  { get; set; }
    public DbSet<Appointment> Appointments  { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    { 
        // Ignore unused Identity tables
        builder.Ignore<IdentityUserClaim<Guid>>();
        builder.Ignore<IdentityRoleClaim<Guid>>();
        builder.Ignore<IdentityUserLogin<Guid>>();
        builder.Ignore<IdentityUserToken<Guid>>(); 

        // Call base to configure Identity tables (includes IdentityUserRole composite key)
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
