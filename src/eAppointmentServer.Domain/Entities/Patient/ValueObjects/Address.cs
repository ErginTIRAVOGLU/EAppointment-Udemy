namespace eAppointmentServer.Domain.Entities.Patient.ValueObjects;

public readonly record struct Address(
    City City,
    Town Town,
    FullAddress FullAddress);
