using eAppointmentServer.Application.Features.Patients;
using Microsoft.AspNetCore.Mvc;
using TS.MediatR;

namespace eAppointmentServer.WebAPI.Controllers;

[Route("api/patients")]
public sealed class PatientController(ISender sender) : BaseApiController(sender)
{
    [HttpGet]
    [Route("")] // GET /api/patients
    public async Task<IActionResult> GetAllPatients(CancellationToken cancellationToken)
    {
        var query = new GetAllPatientsQuery();
        var result = await Sender.Send(query, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet]
    [Route("IdentityNumber/{identityNumber}")] // GET /api/patients/IdentityNumber/{identityNumber}
    public async Task<IActionResult> GetPatientByIdentityNumber(string identityNumber, CancellationToken cancellationToken)
    {
        var query = new GetPatientByIdentityNumberQuery(identityNumber);
        var result = await Sender.Send(query, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPost]
    [Route("")] // POST /api/patients
    public async Task<IActionResult> CreatePatient([FromBody] CreatePatientCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpDelete]
    [Route("{id}")] // DELETE /api/patients/{id}
    public async Task<IActionResult> DeletePatient(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeletePatientCommand(id);
        var result = await Sender.Send(command, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }
    [HttpPut]
    [Route("")] // PUT /api/patients
    public async Task<IActionResult> UpdatePatient([FromBody] UpdatePatientCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }
}
