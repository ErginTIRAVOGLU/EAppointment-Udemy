using System;
using eAppointmentServer.Domain.Entities.Patient;
using eAppointmentServer.Domain.Repositories;
using ErginWebDev.Result;
using GenericRepository;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Patients;

public sealed record DeletePatientCommand(Guid PatientId) : IRequest<Result<string>>;

internal sealed class DeletePatientCommandHandler(
    IPatientRepository patientRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeletePatientCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var patientId = new PatientId(request.PatientId);
        var patient = await patientRepository.GetByExpressionAsync(p=> p.Id == patientId, cancellationToken);
        if (patient is null)
        {
            return Result<string>.Fail("Patient not found.", statusCode: System.Net.HttpStatusCode.NotFound);
        }

        patientRepository.Delete(patient);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Success("Patient deleted successfully.");
    }
}