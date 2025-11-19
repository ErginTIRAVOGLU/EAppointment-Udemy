using eAppointmentServer.Application.Features.Departments;
using Microsoft.AspNetCore.Mvc;
using TS.MediatR;

namespace eAppointmentServer.WebAPI.Controllers;

[Route("api/departments")]
public sealed class DepartmentsController(ISender sender) : BaseApiController(sender)
{
    [HttpGet]
    [Route("")] 
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllDepartmentQuery();
        var result = await Sender.Send(query, cancellationToken);
        return StatusCode((int)result.StatusCode, result);
    }
}
