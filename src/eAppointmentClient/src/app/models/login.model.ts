export interface LoginModel {
    usernameOrEmail: string;
    password: string;
}

export const initialLoginModel: LoginModel = {
    usernameOrEmail: '',
    password: ''
};