export interface CreateAppointmentModel {
   startDate: string, 
   endDate: string,
   doctorId: string,
   patientId: string,
   patientFirstName: string,
   patientLastName: string,
   patientIdentityNumber: string,
   patientCity: string,
   patientTown: string,
   patientFullAddress: string
}

export const initialCreateAppointmentModel: CreateAppointmentModel = {
    startDate: '',
    endDate: '',
    doctorId: '',
    patientId: '',
    patientFirstName: '',
    patientLastName: '',
    patientIdentityNumber: '',
    patientCity: '',
    patientTown: '',
    patientFullAddress: ''
};