using System;
using eAppointmentServer.Application.Features.Doctors;
using Microsoft.AspNetCore.Mvc;
using TS.MediatR;

namespace eAppointmentServer.WebAPI.Controllers;

[Route("api/doctors")]
public sealed class DoctorsController(ISender sender) : BaseApiController(sender)
{
    [HttpGet]
    [Route("")] // GET /api/doctors
    public async Task<IActionResult> GetAllDoctors(CancellationToken cancellationToken)
    {
        var query = new GetAllDoctorQuery();
        var result = await Sender.Send(query, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet]
    [Route("departments/{id}")] // GET /api/doctors/departments/{id}
    public async Task<IActionResult> GetAllDoctorsByDepartment(int id, CancellationToken cancellationToken)
    { 
        var query = new GetAllDoctorsByDepartmentIdQuery(id);
        var result = await Sender.Send(query, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost]
    [Route("")] // POST /api/doctors
    public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpDelete]
    [Route("{id}")] // DELETE /api/doctors/{id}
    public async Task<IActionResult> DeleteDoctor(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteDoctorCommand(id);
        var result = await Sender.Send(command, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }
    
    [HttpPut]
    [Route("")] // PUT /api/doctors
    public async Task<IActionResult> UpdateDoctor([FromBody] UpdateDoctorCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }


}
