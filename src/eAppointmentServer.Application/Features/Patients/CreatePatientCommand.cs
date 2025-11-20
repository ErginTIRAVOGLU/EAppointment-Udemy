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

public sealed record CreatePatientCommand(
    string FirstName,
    string LastName,
    string IdentityNumber,
    string City,
    string Town,
    string FullAddress
) : IRequest<Result<string>>;

internal sealed class CreatePatientCommandHandler(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork
)
    : IRequestHandler<CreatePatientCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        var existsingPatient = await patientRepository
            .GetByExpressionAsync(p => p.IdentityNumber == new IdentityNumber(request.IdentityNumber), cancellationToken);
        if (existsingPatient is not null)
        {
            return Result<string>.Fail("Patient with the same identity number already exists.",statusCode: HttpStatusCode.Conflict);
        }

        var patient = new Patient(
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

        await patientRepository.AddAsync(patient, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(patient.Id.Value.ToString());
    }
}