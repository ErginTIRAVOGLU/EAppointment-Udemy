import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Result } from '../models/result.model';
import { api } from '../constants';
import { ErrorService } from './error-service';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  readonly #token = signal<string>('');
  readonly #http = inject(HttpClient);
  readonly #errorService = inject(ErrorService);

  constructor() {
    this.#token.set(localStorage.getItem('token') || '');
  }

  get<T>(apiUrl: string, callback: (response: Result<T>) => void, errorCallback?: (error: HttpErrorResponse) => void) {
    this.#http.get<Result<T>>(`${api}/${apiUrl}`, {
      headers: { 
        'Authorization': `Bearer ${this.#token()}`
       }
    }).subscribe({
      next: (response) => {
        callback(response);
      },
      error: (err: HttpErrorResponse) => {
        if (errorCallback !== undefined) {
          errorCallback(err); 
        }
        else {
          this.#errorService.errorHandler(err);
        }
      },
    });
  }
  

  post<T>(apiUrl: string, body: any, callback: (response: Result<T>) => void, errorCallback?: (error: HttpErrorResponse) => void) {
    this.#http.post<Result<T>>(`${api}/${apiUrl}`, body, {
      headers: { 
        'Authorization': `Bearer ${this.#token()}`
       }
    }).subscribe({
      next: (response) => {
        callback(response);
      },
      error: (err: HttpErrorResponse) => {
        if (errorCallback !== undefined) {
          errorCallback(err);
          
        }
        else {
          this.#errorService.errorHandler(err);
        }
      },
    });
  }

  put<T>(apiUrl: string, body: any, callback: (response: Result<T>) => void, errorCallback?: (error: HttpErrorResponse) => void) {
    this.#http.put<Result<T>>(`${api}/${apiUrl}`, body, {
      headers: { 
        'Authorization': `Bearer ${this.#token()}`
       }
    }).subscribe({
      next: (response) => {
        callback(response);
      },
      error: (err: HttpErrorResponse) => {
        if (errorCallback !== undefined) {
          errorCallback(err);
          
        }
        else {
          this.#errorService.errorHandler(err);
        }
      },
    });
  }

  delete<T>(apiUrl: string, callback: (response: Result<T>) => void, errorCallback?: (error: HttpErrorResponse) => void) {
    this.#http.delete<Result<T>>(`${api}/${apiUrl}`, {
      headers: { 
        'Authorization': `Bearer ${this.#token()}`
       }
    }).subscribe({
      next: (response) => {
        callback(response);
      },
      error: (err: HttpErrorResponse) => {
       if (errorCallback !== undefined) {
          errorCallback(err);
          
        }
        else {
          this.#errorService.errorHandler(err);
        }
      },
    });
  }
}
