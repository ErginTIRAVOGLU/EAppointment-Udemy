using System;

namespace eAppointmentServer.Application.Features.Patients;

public sealed record GetAllPatientsDto (
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    string IdentityNumber,
    string City,
    string Town,
    string FullAddress
);