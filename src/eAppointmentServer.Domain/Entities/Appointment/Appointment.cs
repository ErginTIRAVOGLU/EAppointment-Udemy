using System;
using eAppointmentServer.Domain.Entities.Appointment.ValueObjects;
using eAppointmentServer.Domain.Entities.Doctor.ValueObjects;
using eAppointmentServer.Domain.Entities.Patient;

namespace eAppointmentServer.Domain.Entities.Appointment;

public sealed class Appointment
{
    public Appointment(AppointmentId id, DoctorId doctorId, PatientId patientId, DateTime startDate, DateTime endDate,  bool isCompleted)
    {
        Id = id;
        SetDoctorId(doctorId);
        SetPatientId(patientId);
        SetStartDate(startDate);
        SetEndDate(endDate);
        SetIsCompleted(isCompleted);
    }

    public AppointmentId Id { get; set; }
    public DoctorId DoctorId { get; set; }
    public PatientId PatientId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCompleted { get; set; }

    #region Methods

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
