using System;
using eAppointmentServer.Domain.Entities.Doctor;
using eAppointmentServer.Domain.Entities.Doctor.ValueObjects;
using eAppointmentServer.Domain.Repositories;
using ErginWebDev.Result;
using GenericRepository;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Doctors;

public sealed record DeleteDoctorCommand(
    Guid Id
) : IRequest<Result<string>>;

internal sealed class DeleteDoctorCommandHandler(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteDoctorCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctorId = new DoctorId(request.Id);
        var doctor = await doctorRepository.GetByExpressionAsync(d => d.Id == doctorId, cancellationToken);
        if (doctor is null)
        {
            return Result<string>.Fail("Doctor not found.");
        }

        doctorRepository.Delete(doctor);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Success("Doctor deleted successfully.");
    }
}
 