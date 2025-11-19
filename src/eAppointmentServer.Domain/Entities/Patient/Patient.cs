using eAppointmentServer.Domain.Entities.Patient.ValueObjects;
using eAppointmentServer.Domain.Entities.Shared.ValueObjects;

namespace eAppointmentServer.Domain.Entities.Patient;

public sealed class Patient
{
    // Parameterless constructor for EF Core
    private Patient() { }

    public Patient(FirstName firstName, LastName lastName, IdentityNumber identityNumber, Address address)
    {
        Id = new PatientId(Guid.CreateVersion7());
        SetFirstName(firstName);
        SetLastName(lastName);
        SetIdentityNumber(identityNumber);
        SetAddress(address);
    }


    public PatientId Id { get; private set; }
    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public string FullName => $"{FirstName.Value} {LastName.Value}";

    public IdentityNumber IdentityNumber { get; private set; }
    public Address Address { get; private set; }

    #region Methods
    
    private void SetFirstName(FirstName firstName)
    {
        FirstName = firstName;
    }

    private void SetLastName(LastName lastName)
    {
        LastName = lastName;
    } 

    private void SetIdentityNumber(IdentityNumber identityNumber)
    {
        IdentityNumber = identityNumber;
    }

    private void SetAddress(Address address)
    {
        Address = address;
    }

    #endregion
}
