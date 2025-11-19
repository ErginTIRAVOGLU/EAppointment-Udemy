using eAppointmentServer.Domain.Entities.Patient;
using eAppointmentServer.Domain.Entities.Patient.ValueObjects;
using eAppointmentServer.Domain.Entities.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAppointmentServer.Infrastructure.Configurations;

public sealed class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(
                v => v.Value,
                v => new PatientId(v))
            .HasColumnName("Id");

        builder.Property(p => p.FirstName)
            .HasConversion(
                v => v.Value,
                v => new FirstName(v))
            .HasColumnName("FirstName")
            .HasColumnType("nvarchar(100)")
            .IsRequired();

        builder.Property(p => p.LastName)
            .HasConversion(
                v => v.Value,
                v => new LastName(v))
            .HasColumnName("LastName")
            .HasColumnType("nvarchar(100)")
            .IsRequired();
        
        builder.ComplexProperty(p => p.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.City)
                .HasConversion(
                    v => v.Value,
                    v => new City(v))
                .HasColumnName("City")
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            addressBuilder.Property(a => a.Town)
                .HasConversion(
                    v => v.Value,
                    v => new Town(v))
                .HasColumnName("Town")
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            addressBuilder.Property(a => a.FullAddress)
                .HasConversion(
                    v => v.Value,
                    v => new FullAddress(v))
                .HasColumnName("FullAddress")
                .HasColumnType("nvarchar(250)")
                .IsRequired();
        });
        
        builder.Property(p => p.IdentityNumber)
            .HasConversion(
                v => v.Value,
                v => new IdentityNumber(v))
            .HasColumnName("IdentityNumber")
            .HasColumnType("nvarchar(11)")
            .IsRequired();
        builder.HasIndex(p => p.IdentityNumber).IsUnique();
    } 
}
