using System;
using System.Net;
using eAppointmentServer.Domain.Entities.Patient;
using eAppointmentServer.Domain.Entities.Patient.ValueObjects;
using eAppointmentServer.Domain.Entities.Shared.ValueObjects;
using eAppointmentServer.Domain.Repositories;
using ErginWebDev.Result;
using GenericRepository;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Patients;

public sealed record UpdatePatientCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string IdentityNumber,
    string City,
    string Town,
    string FullAddress
) : IRequest<Result<string>>;


internal sealed class UpdatePatientCommandHandler(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdatePatientCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var patientId = new PatientId(request.Id);
        var patient = await patientRepository.GetByExpressionAsync(p=> p.Id == patientId, cancellationToken);
        if (patient is null)
        {
            return Result<string>.Fail("Patient not found.", statusCode: HttpStatusCode.NotFound);
        }

        if(patient.IdentityNumber.Value != request.IdentityNumber)
        {
            var existingPatient = await patientRepository
                .GetByExpressionAsync(p => p.IdentityNumber == new IdentityNumber(request.IdentityNumber), cancellationToken);
            if (existingPatient is not null)
            {
                return Result<string>.Fail("Another patient with the same identity number already exists.", statusCode: HttpStatusCode.Conflict);
            }
        }

       patient.Update(
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

        patientRepository.Update(patient);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success("Patient updated successfully.");

    }
}