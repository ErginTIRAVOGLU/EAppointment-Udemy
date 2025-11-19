using eAppointmentServer.Domain.Entities.Doctor.ValueObjects;
using eAppointmentServer.Domain.Entities.Shared.ValueObjects;

namespace eAppointmentServer.Domain.Entities.Doctor;

public sealed class Doctor
{
    // Parameterless constructor for EF Core
    private Doctor() { }
    
    public Doctor(FirstName firstName, LastName lastName, Department department)
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
    public Department Department { get; private set; } 

    #region Methods         

    public void Update(FirstName firstName, LastName lastName, Department department)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetDepartment(department);
    }

    private void SetFirstName(FirstName firstName)
    {
        FirstName = firstName;
    }
    private void SetLastName(LastName lastName)
    {
        LastName = lastName;
    }
    private void SetDepartment(Department department)
    {
        Department = department;
    }

  
    #endregion
}
