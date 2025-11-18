using eAppointmentServer.Domain.Entities.Appointment;
using GenericRepository;

namespace eAppointmentServer.Domain.Repositories;

public interface IAppointmentRepository : IRepository<Appointment>
{

}