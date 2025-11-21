export interface UserModel {
    id: string,
    firstName: string,
    lastName: string,
    fullName: string,
    userName: string,
    email: string,    
    password?: string,
    roleIds?: string[]
}

export const initialUserModel: UserModel = {
    id: '',
    firstName: '',
    lastName: '',
    fullName: '',
    userName: '',
    email: '',    
    roleIds: []
};