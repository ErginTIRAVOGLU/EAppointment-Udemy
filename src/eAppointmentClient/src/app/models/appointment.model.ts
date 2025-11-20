export interface AppointmentModel {
    id: string,
    startDate: string,
    endDate: string,
    title:string ,
    patientFullName:string ,
    patientIdentityNumber:string ,
    patientAddress:string,
    patientId?:string,
    doctorId?:string
}

export const initialAppointmentModel: AppointmentModel = {
    id: '',
    startDate: '',
    endDate: '',
    title: '',
    patientFullName: '',
    patientIdentityNumber: '',
    patientAddress: '',
    patientId: '',
    doctorId: ''
};