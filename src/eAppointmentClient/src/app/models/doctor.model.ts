export interface DoctorModel {
    id: string;
    firstName: string;
    lastName: string;
    fullName: string;
    department: number | '';
    departmentValue: number | '';
    departmentName?: string; 
}

export const initialDoctorModel: DoctorModel = {
    id: '',
    firstName: '',
    lastName: '',
    fullName: '',
    department: '',
    departmentValue: '',
    departmentName: '',
};