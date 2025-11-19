using System;
using eAppointmentServer.Domain.Entities.Doctor;
using eAppointmentServer.Domain.Entities.Doctor.ValueObjects;
using eAppointmentServer.Domain.Entities.Shared.ValueObjects;
using eAppointmentServer.Domain.Enums;
using eAppointmentServer.Domain.Repositories;
using ErginWebDev.Result;
using GenericRepository;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Doctors;

public sealed record CreateDoctorCommand(
    string FirstName,
    string LastName,
    int Department
) : IRequest<Result<string>>;

internal sealed class CreateDoctorCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateDoctorCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
         Doctor doctor = new Doctor(
            firstName: new FirstName(request.FirstName),
            lastName: new LastName(request.LastName),
            department: new Department(DepartmentEnum.FromValue(request.Department))
        );

        await doctorRepository.AddAsync(doctor, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Success("Doctor created successfully.");
    }
}