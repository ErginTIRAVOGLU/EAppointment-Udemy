namespace eAppointmentServer.Application.Features.Appoinments;

public sealed record GetAllAppointmentsDto(
    Guid Id,
    DateTime StartDate,
    DateTime EndDate,
    string Title,
    string PatientFullName,
    string PatientIdentityNumber,
    string PatientAddress
);
