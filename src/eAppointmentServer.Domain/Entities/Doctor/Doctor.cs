using System;
using eAppointmentServer.Domain.Entities.Doctor.ValueObjects;
using eAppointmentServer.Domain.Entities.Shared.ValueObjects;

namespace eAppointmentServer.Domain.Entities.Doctor;

public sealed class Doctor
{
    public Doctor(FirstName firstName, LastName lastName, DepartmentId department)
    {
        Id = new DoctorId(Guid.CreateVersion7());
        SetFirstName(firstName);
        SetLastName(lastName);
        SetDepartment(department);
    }

    public DoctorId Id { get; private set; }
    public FirstName FirstName { get; private set; } 
    public LastName LastName { get; private set; } 
    public string FullName => $"{FirstName.Value} {LastName.Value}";
    public DepartmentId Department { get; private set; } 

    #region Methods
    private void SetFirstName(FirstName firstName)
    {
        FirstName = firstName;
    }
    private void SetLastName(LastName lastName)
    {
        LastName = lastName;
    }
    private void SetDepartment(DepartmentId department)
    {
        Department = department;
    }
    #endregion
}
