using System;
using eAppointmentServer.Domain.Entities.Patient;
using eAppointmentServer.Domain.Repositories;
using ErginWebDev.Result;
using Microsoft.EntityFrameworkCore;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Patients;

public sealed record GetAllPatientsQuery() : IRequest<Result<List<GetAllPatientsDto>>>;

internal sealed class GetAllPatientsQueryHandler(
    IPatientRepository patientRepository
) : IRequestHandler<GetAllPatientsQuery, Result<List<GetAllPatientsDto>>>
{
    public async Task<Result<List<GetAllPatientsDto>>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
    {
        List<Patient> patientEntities = await patientRepository
            .GetAll()
            .ToListAsync(cancellationToken);

            List<GetAllPatientsDto> patients = patientEntities
            .OrderBy(p => p.FirstName.Value)
            .ThenBy(p => p.LastName.Value)
            .Select(d => new GetAllPatientsDto(
                d.Id.Value,
                d.FirstName.Value,
                d.LastName.Value,
                d.FullName,
                d.IdentityNumber.Value,
                d.Address.City.Value,
                d.Address.Town.Value,
                d.Address.FullAddress.Value
            ))
            .ToList();
            
        
        return Result<List<GetAllPatientsDto>>.Success(patients);
    }
}