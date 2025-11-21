export interface UpdateAppointmentModel {
    id: string,
    startDate: string,
    endDate: string,
    title:string ,
    fullName:string ,
    firstName:string ,
    lastName:string ,
    identityNumber:string ,
    address:string,
    city:string,
    town:string,
    fullAddress:string,
    patientId?:string,
    doctorId?:string
    doctorFullName?:string
}

export const initialUpdateAppointmentModel: UpdateAppointmentModel = {
    id: '',
    startDate: '',
    endDate: '',
    title: '',
    fullName: '',
    firstName: '',
    lastName: '',
    identityNumber: '',
    address: '',
    city: '',
    town: '',
    fullAddress: '',
    patientId: '',
    doctorId: '',
    doctorFullName: ''
};