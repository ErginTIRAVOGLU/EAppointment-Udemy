export interface CreateAppointmentModel {
   startDate: string, 
   endDate: string,
   doctorId: string,
   patientId: string,
   FirstName: string,
   LastName: string,
   IdentityNumber: string,
   City: string,
   Town: string,
   FullAddress: string
}

export const initialCreateAppointmentModel: CreateAppointmentModel = {
    startDate: '',
    endDate: '',
    doctorId: '',
    patientId: '',
    FirstName: '',
    LastName: '',
    IdentityNumber: '',
    City: '',
    Town: '',
    FullAddress: ''
};