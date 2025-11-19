using System;
using System.Linq;
using eAppointmentServer.Domain.Entities.Doctor;
using eAppointmentServer.Domain.Repositories;
using ErginWebDev.Result;
using Microsoft.EntityFrameworkCore;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Doctors;

public sealed record GetAllDoctorsByDepartmentIdQuery(
    int DepartmentId
) : IRequest<Result<List<GetAllDoctorDto>>>;

internal sealed class GetAllDoctorsByDepartmentIdQueryHandler(
    IDoctorRepository doctorRepository
) : IRequestHandler<GetAllDoctorsByDepartmentIdQuery, Result<List<GetAllDoctorDto>>>
{
    public async Task<Result<List<GetAllDoctorDto>>> Handle(GetAllDoctorsByDepartmentIdQuery request, CancellationToken cancellationToken)
    {
          List<Doctor> doctorEntities = await doctorRepository
            .GetAll()            
            .ToListAsync(cancellationToken);
        
        // Map to DTOs in memory (client-side)
        List<GetAllDoctorDto> doctors = doctorEntities 
            .Where(d => d.Department.Value.Value == request.DepartmentId)
            .OrderBy(p => p.Department.Value.Value)
            .ThenBy(p => p.FirstName.Value)
            .Select(d => new GetAllDoctorDto(
                d.Id.Value,
                d.FirstName.Value,
                d.LastName.Value,
                d.FullName,
                d.Department.Value.Name,
                d.Department.Value.Value
            ))
            .ToList();
        return Result<List<GetAllDoctorDto>>.Success(doctors);
    }
}