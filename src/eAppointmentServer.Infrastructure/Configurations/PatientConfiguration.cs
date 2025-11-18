using eAppointmentServer.Domain.Entities.Patient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAppointmentServer.Infrastructure.Configurations;

public sealed class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.Property(d => d.FirstName).HasColumnType("nvarchar(100)").IsRequired();
        builder.Property(d => d.LastName).HasColumnType("nvarchar(100)").IsRequired();
        
        builder.ComplexProperty(p => p.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.City).HasColumnName("City").HasColumnType("nvarchar(100)").IsRequired();
            addressBuilder.Property(a => a.Town).HasColumnName("Town").HasColumnType("nvarchar(100)").IsRequired();
            addressBuilder.Property(a => a.FullAddress).HasColumnName("FullAddress").HasColumnType("nvarchar(250)").IsRequired();
        });
        
        builder.Property(d => d.IdentityNumber).HasColumnType("nvarchar(11)").IsRequired();
        builder.HasIndex(d => d.IdentityNumber).IsUnique();
    } 
}
