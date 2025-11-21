using System;
using eAppointmentServer.Application.Features.Appoinments;
using Microsoft.AspNetCore.Mvc;
using TS.MediatR;

namespace eAppointmentServer.WebAPI.Controllers;

[Route("api/appointments")]
public class AppointmentsController(ISender sender) : BaseApiController(sender)
{

    [HttpGet]
    [Route("{id}")] // GET /api/appointments/{id}
    public async Task<IActionResult> GetAppointmentById(Guid id, CancellationToken cancellationToken)
    { 
        var query = new GetAppointmentQuery(id);
        var result = await Sender.Send(query, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet]
    [Route("doctor/{id}")] // GET /api/appointments/doctor/{id}
    public async Task<IActionResult> GetAllAppointmentsByDoctorId(Guid id, CancellationToken cancellationToken)
    { 
        var query = new GetAllAppointmentsByDoctorIdQuery(id);
        var result = await Sender.Send(query, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost]
    [Route("")] // POST /api/appointments
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpDelete]
    [Route("{id}")] // DELETE /api/appointments/{id}
    public async Task<IActionResult> DeleteAppointment(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteAppointmentCommand(id);
        var result = await Sender.Send(command, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut]
    [Route("")] // PUT /api/appointments
    public async Task<IActionResult> UpdateAppointment([FromBody] UpdateAppointmentCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return StatusCode((int)result.StatusCode, result);  
    }
}
