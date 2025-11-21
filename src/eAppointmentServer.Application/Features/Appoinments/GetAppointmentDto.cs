using eAppointmentServer.Domain.Entities.Appointment.ValueObjects;
using eAppointmentServer.Domain.Entities.Doctor.ValueObjects;
using eAppointmentServer.Domain.Entities.Patient;
using eAppointmentServer.Domain.Entities.Patient.ValueObjects;

namespace eAppointmentServer.Application.Features.Appoinments;

public sealed record GetAppointmentDto(
    Guid Id,
    Guid DoctorId,
    string DoctorFullName,
    Guid PatientId,
    string FullName,
    string FirstName,
    string LastName,
    string IdentityNumber,
    DateTime StartDate,
    DateTime EndDate,
    string City,
    string Town,
    string FullAddress,
    bool IsCompleted
);
