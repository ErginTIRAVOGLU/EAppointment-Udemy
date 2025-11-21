using System;
using eAppointmentServer.Domain.Entities.Appointment.ValueObjects;
using eAppointmentServer.Domain.Entities.Doctor.ValueObjects;
using eAppointmentServer.Domain.Entities.Patient;
using eAppointmentServer.Domain.Repositories;
using ErginWebDev.Result;
using GenericRepository;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Appoinments;

public sealed record UpdateAppointmentCommand(
    Guid Id,
    DateTime StartDate,
    DateTime EndDate,
    Guid PatientId,
    Guid DoctorId
) : IRequest<Result<string>>;

internal sealed class UpdateAppointmentCommandHandler(
    IAppointmentRepository appointmentRepository,
    IPatientRepository patientRepository,
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateAppointmentCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointmentId = new AppointmentId(request.Id);
        var appointment = appointmentRepository.Where(a => a.Id == appointmentId).FirstOrDefault();
        if (appointment == null)
        {
            return Result<string>.Fail("Appointment not found.");
        }

        var doctorId = new DoctorId(request.DoctorId);
        var doctor = doctorRepository.Where(d => d.Id == doctorId).FirstOrDefault();
        if (doctor == null)
        {
            return Result<string>.Fail("Doctor not found.");
        }

        var patientId = new PatientId(request.PatientId);
        var patient = patientRepository.Where(p => p.Id == patientId).FirstOrDefault();
        if (patient == null)
        {
            return Result<string>.Fail("Patient not found.");
        }

        DateTime StartDate = request.StartDate;;
        DateTime EndDate = request.EndDate;
      

        appointment.Update(
            doctorId,
            patientId,
            StartDate,
            EndDate,
            false
        );

        
        appointmentRepository.Update(appointment);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success("Appointment updated successfully.");
    }
}