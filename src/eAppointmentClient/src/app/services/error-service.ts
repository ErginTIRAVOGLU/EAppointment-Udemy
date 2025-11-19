import { HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { SwalService } from './swal-service';

@Injectable({
  providedIn: 'root',
})
export class ErrorService {
  readonly #swal = inject(SwalService);

  errorHandler(error: HttpErrorResponse) {
    console.log('Error occurred:', error);
    
     let message = 'An unknown error occurred!';
     if(error.status === 0){
       message = 'Cannot connect to the server. Please check your internet connection.';
     } else if (error.status ===400) {
       message = 'Bad Request. Please verify your input.';
     } else if (error.status === 401) {
       message = 'Unauthorized access. Please log in.';
     } else if (error.status === 403) {
       message = 'Forbidden. You do not have permission to access this resource.';
     } else if (error.status === 404) {
       message = 'Resource not found. Please check the URL.';
     } else if (error.status === 500) {
       message = 'Internal Server Error. Please try again later.';
     } else if (error.error && error.error.message) {
       message = error.error.message;
     }

     this.#swal.callToast(message, 'error', 5000);
  }
}
