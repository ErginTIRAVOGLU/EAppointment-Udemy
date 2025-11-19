using eAppointmentServer.Domain.Entities.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace eAppointmentServer.Domain.Entities.AppUser;

public sealed class AppUser : IdentityUser<Guid>
{
    // Parameterless constructor for EF Core
    private AppUser() { }
    
    public AppUser(FirstName firstName, LastName lastName)
    {
        Id = Guid.CreateVersion7();
        SetFirstName(firstName);
        SetLastName(lastName);
    }
    public FirstName FirstName { get; private set; } 
    public LastName LastName { get; private set; } 
    public string FullName => $"{FirstName.Value} {LastName.Value}";
    
    #region Methods
    
    private void SetFirstName(FirstName firstName)
    {
        FirstName = firstName;
    }
    
    private void SetLastName(LastName lastName)
    {
        LastName = lastName;
    }
    
    #endregion
}
