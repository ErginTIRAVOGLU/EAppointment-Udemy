export interface TokenModel {
    id?: string | undefined;
    userid?: string | undefined;
    name: string;
    email: string;
    userName: string;
    exp: number | undefined;
 
}

export const initialTokenModel: TokenModel = {
    id: '',
    userid: '',
    name: '',
    email: '',
    userName: '',
    exp: -1
};