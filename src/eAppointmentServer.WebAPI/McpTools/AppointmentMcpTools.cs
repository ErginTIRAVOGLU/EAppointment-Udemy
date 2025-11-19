using System.ComponentModel;
using System.Net;
using eAppointmentServer.Application.Features.Doctors;
using eAppointmentServer.Domain.Entities.Doctor;
using eAppointmentServer.Domain.Entities.Doctor.ValueObjects;
using eAppointmentServer.Domain.Entities.Shared.ValueObjects;
using eAppointmentServer.Domain.Enums;
using eAppointmentServer.Domain.Repositories;
using ErginWebDev.Result;
using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Server;
using TS.MediatR;

namespace eAppointmentServer.WebAPI.McpTools;

[McpServerToolType]
public class AppointmentMcpTools(    
    ISender sender
    )
{
    [McpServerTool, Description("Echoes the message back to the client.")]
    public static string Echo(string message) => $"hello {message}";

    [McpServerTool, Description("Get all doctors")]
    public async Task<Result<List<GetAllDoctorDto>>> GetAllDoctors(CancellationToken cancellationToken)
    {
       var query = new GetAllDoctorQuery();
        var result = await sender.Send(query, cancellationToken);
        return result;
    }

    [McpServerTool, Description("Get all doctors filtered by department ID.")]
    public async Task<Result<List<GetAllDoctorDto>>> GetAllDoctorsFilterByDepartment(int departmentId, CancellationToken cancellationToken)
    {
        var query = new GetAllDoctorsByDepartmentIdQuery(departmentId);
        var result = await sender.Send(query, cancellationToken);
        return result;
    }

    [McpServerTool, Description("Get all departments")]
    public IEnumerable<DepartmentEnum> GetAllDepartments()
    {
        return DepartmentEnum.List;
    }

    [McpServerTool, Description("Create a new doctor, given first name, last name, and department value.")]
    public async Task<string> CreateDoctor(string firstName, string lastName, int departmentValue)
    {
        var command = new CreateDoctorCommand(firstName, lastName, departmentValue);
        var result = await sender.Send(command);
        return result.StatusCode == HttpStatusCode.OK ? "Doctor created successfully." : "Error creating doctor";
    }

}
