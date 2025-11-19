using eAppointmentServer.Domain.Enums;
using ErginWebDev.Result;
using TS.MediatR;

namespace eAppointmentServer.Application.Features.Departments;

public sealed record GetAllDepartmentQuery() : IRequest<Result<List<GetAllDepartmentDto>>>;

internal sealed class GetAllDepartmentQueryHandler 
    : IRequestHandler<GetAllDepartmentQuery, Result<List<GetAllDepartmentDto>>>
{
    public Task<Result<List<GetAllDepartmentDto>>> Handle(GetAllDepartmentQuery request, CancellationToken cancellationToken)
    {
        List<GetAllDepartmentDto> departments = DepartmentEnum.List
            .Select(d => new GetAllDepartmentDto(
                d.Value,
                d.Name
            ))
            .OrderBy(d => d.Value)
            .ToList();

        return Task.FromResult(Result<List<GetAllDepartmentDto>>.Success(departments));
    }
}
