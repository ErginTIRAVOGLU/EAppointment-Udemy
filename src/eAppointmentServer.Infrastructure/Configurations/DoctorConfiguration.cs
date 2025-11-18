using System;
using eAppointmentServer.Domain.Entities.Doctor;
using eAppointmentServer.Domain.Entities.Patient.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAppointmentServer.Infrastructure.Configurations;

public sealed class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.Property(d => d.FirstName).HasColumnType("nvarchar(100)").IsRequired();
        builder.Property(d => d.LastName).HasColumnType("nvarchar(100)").IsRequired();
        
    } 
}
