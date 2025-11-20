using System;
using eAppointmentServer.Application.Features.Appoinments;
using Microsoft.AspNetCore.Mvc;
using TS.MediatR;

namespace eAppointmentServer.WebAPI.Controllers;

[Route("api/appointments")]
public class AppointmentsController(ISender sender) : BaseApiController(sender)
{
    [HttpGet]
    [Route("doctor/{id}")] // GET /api/appointments/doctor/{id}
    public async Task<IActionResult> GetAllAppointmentsByDoctorId(Guid id, CancellationToken cancellationToken)
    { 
        var query = new GetAllAppointmentsByDoctorIdQuery(id);
        var result = await Sender.Send(query, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }
}
