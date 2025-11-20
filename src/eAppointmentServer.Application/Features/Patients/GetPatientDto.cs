namespace eAppointmentServer.Application.Features.Patients;

public sealed record GetPatientDto(
    Guid Id,
    string FirstName,
    string LastName,
    string IdentityNumber,
    string FullName,
    string City,
    string Town,
    string FullAddress
);
