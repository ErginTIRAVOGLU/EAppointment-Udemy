namespace eAppointmentServer.Application.Features.Users;

public sealed record GetAllUsersDto(Guid Id, string UserName, string Email, string FirstName, string LastName);