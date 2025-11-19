using System;
using Microsoft.AspNetCore.Mvc;
using TS.MediatR;

namespace eAppointmentServer.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseApiController(ISender sender) : ControllerBase
{
    protected ISender Sender { get; } = sender;
}
