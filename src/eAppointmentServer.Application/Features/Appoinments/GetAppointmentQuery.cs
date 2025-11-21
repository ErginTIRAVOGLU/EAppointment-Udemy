using System;
using eAppointmentServer.Domain.Entities.Appointment;
using eAppointmentServer.Domain.Entities.Appointment.ValueObjects;
using eAppointmentServer.Domain.Entities.Doctor;
using eAppointmentServer.Domain.Entities.Patient;
using eAppointmentServer.Domain.Repositories;
using ErginWebDev.Result;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Appoinments;

public sealed record GetAppointmentQuery(
    Guid Id
) : IRequest<Result<GetAppointmentDto>>;

internal sealed class GetAppointmentQueryHandler(
    IAppointmentRepository appointmentRepository,
    IPatientRepository patientRepository,
    IDoctorRepository doctorRepository
)
    : IRequestHandler<GetAppointmentQuery, Result<GetAppointmentDto>>
{
    public Task<Result<GetAppointmentDto>> Handle(GetAppointmentQuery request, CancellationToken cancellationToken)
    {
        Appointment? appointment = appointmentRepository
            .Where(a => a.Id == new AppointmentId(request.Id))
            .FirstOrDefault();
        
        if (appointment == null)
        {
            return Task.FromResult(Result<GetAppointmentDto>.Fail("Appointment not found."));
        }

        Patient? patient = patientRepository
            .Where(p => p.Id == appointment.PatientId)
            .FirstOrDefault();
        
        if (patient == null)
        {
            return Task.FromResult(Result<GetAppointmentDto>.Fail("Patient not found."));
        }

        Doctor? doctor = doctorRepository
            .Where(d => d.Id == appointment.DoctorId)
            .FirstOrDefault();

        if (doctor == null)
        {
            return Task.FromResult(Result<GetAppointmentDto>.Fail("Doctor not found."));
        }


        var appointmentDto = new GetAppointmentDto(
            appointment.Id.Value,
            appointment.DoctorId.Value,
            doctor.FullName,
            appointment.PatientId.Value,
            patient.FullName,
            patient.FirstName.Value,
            patient.LastName.Value,
            patient.IdentityNumber.Value,
            appointment.StartDate,
            appointment.EndDate,
            patient.Address.City.Value,
            patient.Address.Town.Value,
            patient.Address.FullAddress.Value,
            appointment.IsCompleted
        );
        return Task.FromResult(Result<GetAppointmentDto>.Success(appointmentDto));
    }
}