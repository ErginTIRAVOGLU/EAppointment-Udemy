using eAppointmentServer.Domain.Entities.Doctor;
using eAppointmentServer.Domain.Entities.Doctor.ValueObjects;
using eAppointmentServer.Domain.Entities.Shared.ValueObjects;
using eAppointmentServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAppointmentServer.Infrastructure.Configurations;

public sealed class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id)
            .HasConversion(
                v => v.Value,
                v => new DoctorId(v))
            .HasColumnName("Id");

        builder.Property(d => d.FirstName)
            .HasConversion(
                v => v.Value,
                v => new FirstName(v))
            .HasColumnName("FirstName")
            .HasColumnType("nvarchar(100)")
            .IsRequired();

        builder.Property(d => d.LastName)
            .HasConversion(
                v => v.Value,
                v => new LastName(v))
            .HasColumnName("LastName")
            .HasColumnType("nvarchar(100)")
            .IsRequired();

        builder.Property(d => d.Department)
            .HasConversion(
                v => v.Value.Value,
                v => new Department(DepartmentEnum.FromValue(v)))
            .HasColumnName("Department")
            .HasColumnType("int")
            .IsRequired();
    } 
}
