using System;
using eAppointmentServer.Domain.Entities.Doctor.ValueObjects;
using eAppointmentServer.Domain.Entities.Shared.ValueObjects;
using eAppointmentServer.Domain.Enums;
using eAppointmentServer.Domain.Repositories;
using ErginWebDev.Result;
using GenericRepository;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Doctors;

public sealed record UpdateDoctorCommand(
    Guid Id,
    string FirstName,
    string LastName,
    int DepartmentValue
) : IRequest<Result<string>>;

internal sealed class UpdateDoctorCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateDoctorCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctorId = new DoctorId(request.Id);
        var doctor = await doctorRepository.GetByExpressionAsync(d => d.Id == doctorId, cancellationToken);
        if (doctor is null)
        {
            return Result<string>.Fail("Doctor not found.");
        }

        doctor.Update(
            firstName: new FirstName(request.FirstName),
            lastName: new LastName(request.LastName),
            department: new Department(DepartmentEnum.FromValue(request.DepartmentValue))
        );

        doctorRepository.Update(doctor);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Success("Doctor updated successfully.");
    }
}