using eAppointmentServer.Domain.Entities.Appointment.ValueObjects;
using eAppointmentServer.Domain.Entities.Doctor.ValueObjects;
using eAppointmentServer.Domain.Entities.Patient;

namespace eAppointmentServer.Domain.Entities.Appointment;

public sealed class Appointment
{
    // Parameterless constructor for EF Core
    private Appointment() { }
    
    public Appointment(DoctorId doctorId, PatientId patientId, DateTime startDate, DateTime endDate,  bool isCompleted)
    {
        Id =  new AppointmentId(Guid.CreateVersion7());
        SetDoctorId(doctorId);
        SetPatientId(patientId);
        SetStartDate(startDate);
        SetEndDate(endDate);
        SetIsCompleted(isCompleted);
    }

    public AppointmentId Id { get; private  set; }
    public DoctorId DoctorId { get; private set; }
    public PatientId PatientId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsCompleted { get; private set; }
    
    #region Methods
    public void Update(DoctorId doctorId, PatientId patientId, DateTime startDate, DateTime endDate,  bool isCompleted)
    { 
        SetDoctorId(doctorId);
        SetPatientId(patientId);
        SetStartDate(startDate);
        SetEndDate(endDate);
        SetIsCompleted(isCompleted);
    }
 
    private void SetDoctorId(DoctorId doctorId)
    {
        DoctorId = doctorId;
    }

    private void SetPatientId(PatientId patientId)
    {
        PatientId = patientId;
    }

    private void SetStartDate(DateTime startDate)
    {
        StartDate = startDate;
    }

    private void SetEndDate(DateTime endDate)
    {
        EndDate = endDate;
    }
    
    private void SetIsCompleted(bool isCompleted)
    {
        IsCompleted = isCompleted;
    }
    #endregion
}
