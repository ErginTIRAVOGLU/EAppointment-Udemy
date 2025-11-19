using eAppointmentServer.Domain.Entities.AppUser;
using eAppointmentServer.Domain.Entities.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAppointmentServer.Infrastructure.Configurations;

public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(u => u.FirstName)
            .HasConversion(
                v => v.Value,
                v => new FirstName(v))
            .HasColumnName("FirstName")
            .HasColumnType("nvarchar(100)")
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasConversion(
                v => v.Value,
                v => new LastName(v))
            .HasColumnName("LastName")
            .HasColumnType("nvarchar(100)")
            .IsRequired();
    }
}
