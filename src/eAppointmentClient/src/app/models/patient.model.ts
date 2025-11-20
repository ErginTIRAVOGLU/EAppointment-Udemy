export interface PatientModel {
    id:string;
    firstName: string;
    lastName: string;
    fullName: string;
    city: string;
    town: string;
    fullAddress: string;
    identityNumber: string;
}

export const initialPatientModel: PatientModel = {
    id: '',
    firstName: '',
    lastName: '',
    fullName: '',
    city: '',
    town: '',
    fullAddress: '',
    identityNumber: ''
};