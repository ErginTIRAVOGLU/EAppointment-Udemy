using System;
using eAppointmentServer.Domain.Entities.Appointment;
using eAppointmentServer.Domain.Entities.Doctor.ValueObjects;
using eAppointmentServer.Domain.Entities.Patient;
using eAppointmentServer.Domain.Entities.Patient.ValueObjects;
using eAppointmentServer.Domain.Entities.Shared.ValueObjects;
using eAppointmentServer.Domain.Repositories;
using ErginWebDev.Result;
using GenericRepository;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Appoinments;

public sealed record CreateAppointmentCommand(
    DateTime StartDate,
    DateTime EndDate,
    Guid? PatientId,
    Guid DoctorId,
    string FirstName,
    string LastName,
    string IdentityNumber,
    string City,
    string Town,
    string FullAddress
): IRequest<Result<string>>;
 
 internal sealed class CreateAppointmentCommandHandler(
    IAppointmentRepository appointmentRepository,
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork
 ) : IRequestHandler<CreateAppointmentCommand, Result<string>> 
 {
    public async Task<Result<string>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
 
       if(request.PatientId is null)
       {
            Patient patient = new Patient(  
                new FirstName(request.FirstName),
                new LastName(request.LastName),
                new IdentityNumber(request.IdentityNumber),
                new Address
                {
                    City = new City(request.City),
                    Town = new Town(request.Town),
                    FullAddress = new FullAddress(request.FullAddress)
                }
            );

            await  patientRepository.AddAsync(patient, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            Appointment appointment = new Appointment(
                new DoctorId(request.DoctorId),
                new PatientId(patient.Id.Value),
                request.StartDate,
                request.EndDate,
                false              
            );

            await appointmentRepository.AddAsync(appointment, cancellationToken);
          
       }
       else
       {
            Appointment appointment = new Appointment(
                new DoctorId(request.DoctorId),
                new PatientId(request.PatientId.Value),
                request.StartDate,
                request.EndDate,
                false              
            );

            await appointmentRepository.AddAsync(appointment, cancellationToken);
            
       }

        await unitOfWork.SaveChangesAsync(cancellationToken); 
        return Result<string>.Success("Appointment created successfully.");
    }
 }