using System;
using eAppointmentServer.Domain.Entities.Appointment.ValueObjects;
using eAppointmentServer.Domain.Repositories;
using ErginWebDev.Result;
using GenericRepository;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Appoinments;

public sealed record DeleteAppointmentCommand(
    Guid Id
) : IRequest<Result<string>>;

internal sealed class DeleteAppointmentCommandHandler(
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteAppointmentCommand, Result<string>>
{
    public Task<Result<string>> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointmentId = new AppointmentId(request.Id);
        var appointment = appointmentRepository.Where(a => a.Id == appointmentId).FirstOrDefault();
        if (appointment == null)
        {
            return Task.FromResult(Result<string>.Fail("Appointment not found."));
        }
        appointmentRepository.Delete(appointment);
        unitOfWork.SaveChanges();
        return Task.FromResult(Result<string>.Success("Appointment deleted successfully."));
    }
}