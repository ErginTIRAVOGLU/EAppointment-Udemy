using System;
using eAppointmentServer.Domain.Entities.Appointment;
using eAppointmentServer.Domain.Entities.Doctor.ValueObjects;
using eAppointmentServer.Domain.Entities.Patient;
using eAppointmentServer.Domain.Repositories;
using ErginWebDev.Result;
using Microsoft.EntityFrameworkCore;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Appoinments;

public sealed record GetAllAppointmentsByDoctorIdQuery(
    Guid DoctorId
) : IRequest<Result<List<GetAllAppointmentsDto>>>;

internal sealed class GetAllAppointmentsByDoctorIdQueryHandler(
    IAppointmentRepository appointmentRepository,
    IPatientRepository patientRepository
) : IRequestHandler<GetAllAppointmentsByDoctorIdQuery, Result<List<GetAllAppointmentsDto>>>
{
    public async Task<Result<List<GetAllAppointmentsDto>>> Handle(GetAllAppointmentsByDoctorIdQuery request, CancellationToken cancellationToken)
    {
        // Get appointments for the doctor
        var appointments = await appointmentRepository
            .Where(a => a.DoctorId == new DoctorId(request.DoctorId))
            .ToListAsync(cancellationToken);

        // Get patient IDs from appointments
        var patientIds = appointments.Select(a => a.PatientId).ToList();

        // Get patients data
        var patients = await patientRepository
            .Where(p => patientIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        // Join appointments with patients
        var appointmentDtos = appointments
            .Join(patients,
                appointment => appointment.PatientId,
                patient => patient.Id,
                (appointment, patient) => new GetAllAppointmentsDto(
                    appointment.Id.Value,
                    appointment.StartDate,
                    appointment.EndDate,
                    $"Appointment with {patient.FullName}",
                    $"{patient.FullName}",
                    patient.IdentityNumber.Value,
                    $"{patient.Address.City.Value}, {patient.Address.Town.Value}, {patient.Address.FullAddress.Value}"
                ))
            .ToList();

        return Result<List<GetAllAppointmentsDto>>.Success(appointmentDtos);
    }
}