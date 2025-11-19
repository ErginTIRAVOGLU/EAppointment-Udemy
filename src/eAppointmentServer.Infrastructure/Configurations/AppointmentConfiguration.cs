using eAppointmentServer.Domain.Entities.Appointment;
using eAppointmentServer.Domain.Entities.Appointment.ValueObjects;
using eAppointmentServer.Domain.Entities.Doctor.ValueObjects;
using eAppointmentServer.Domain.Entities.Patient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAppointmentServer.Infrastructure.Configurations;

public sealed class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasConversion(
                v => v.Value,
                v => new AppointmentId(v))
            .HasColumnName("Id");

        builder.Property(a => a.DoctorId)
            .HasConversion(
                v => v.Value,
                v => new DoctorId(v))
            .HasColumnName("DoctorId")
            .IsRequired();

        builder.Property(a => a.PatientId)
            .HasConversion(
                v => v.Value,
                v => new PatientId(v))
            .HasColumnName("PatientId")
            .IsRequired();

        builder.Property(a => a.StartDate)
            .HasColumnName("StartDate")
            .IsRequired();

        builder.Property(a => a.EndDate)
            .HasColumnName("EndDate")
            .IsRequired();

        builder.Property(a => a.IsCompleted)
            .HasColumnName("IsCompleted")
            .IsRequired();
    } 
}