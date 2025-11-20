using System;
using System.Net;
using eAppointmentServer.Domain.Entities.Patient;
using eAppointmentServer.Domain.Entities.Patient.ValueObjects;
using eAppointmentServer.Domain.Repositories;
using ErginWebDev.Result;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Patients;

public sealed record   GetPatientByIdentityNumberQuery(
    string IdentityNumber
) : IRequest<Result<GetPatientDto>>;


internal sealed class GetPatientByIdentityNumberQueryHandler(
    IPatientRepository patientRepository
) : IRequestHandler<GetPatientByIdentityNumberQuery, Result<GetPatientDto>>
{
    public async Task<Result<GetPatientDto>> Handle(GetPatientByIdentityNumberQuery request, CancellationToken cancellationToken)
    {
        var patient = await patientRepository.GetByExpressionAsync(p => p.IdentityNumber == new IdentityNumber(request.IdentityNumber), cancellationToken);
        if (patient is null)
        {
            return Result<GetPatientDto>.Fail("Patient not found.",statusCode:HttpStatusCode.NotFound);
        }

        var patientDto = new GetPatientDto(
            patient.Id.Value,
            patient.FirstName.Value,
            patient.LastName.Value,
            patient.IdentityNumber.Value,
            patient.FullName,
            patient.Address.City.Value,
            patient.Address.Town.Value,
            patient.Address.FullAddress.Value
        );

        return Result<GetPatientDto>.Success(patientDto);
    }
}