import { inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { initialTokenModel, TokenModel } from '../models/token.model';
import { jwtDecode, JwtPayload } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  readonly #router = inject(Router);

  readonly tokenModel = signal<TokenModel>(initialTokenModel);

  isAuthenticated() {
    const token: string | null = localStorage.getItem('token');

    if (token) {
      const decodedToken :any = jwtDecode(token);
      const newTokenModel: TokenModel = {
        id: decodedToken.jti,
        userid: decodedToken.sub,
        name: decodedToken.name ?? decodedToken["name"],
        email: decodedToken.email ?? decodedToken["email"],
        userName: decodedToken.UserName ?? decodedToken["UserName"],
        exp: decodedToken.exp ?? decodedToken["exp"]
      };

      const now= new Date().getTime() / 1000;
      if(newTokenModel.exp !== undefined && newTokenModel.exp < now){
        
        localStorage.removeItem('token');
        this.tokenModel.set(initialTokenModel);
        return false;
      }

      this.tokenModel.set(newTokenModel);



      return true;
    }
    return false;
  }
}
