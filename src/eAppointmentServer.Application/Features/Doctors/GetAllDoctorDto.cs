namespace eAppointmentServer.Application.Features.Doctors;

public sealed record GetAllDoctorDto(
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    string DepartmentName,
    int DepartmentValue
);
