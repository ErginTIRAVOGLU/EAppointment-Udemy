export interface TokenModel {
    id?: string | undefined;
    userid?: string | undefined;
    name: string;
    email: string;
    userName: string;
    exp: number | undefined;
    roles: string[];
 
}

export const initialTokenModel: TokenModel = {
    id: '',
    userid: '',
    name: '',
    email: '',
    userName: '',
    exp: -1,
    roles: []
};