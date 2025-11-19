import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';


@Injectable({
  providedIn: 'root',
})
export class SwalService {
  callToast(text: string, icon: SweetAlertIcon = 'success', timer = 3000) {
    Swal.fire({
      text: text,
      timer: timer,
      icon: icon,
      position: 'top-end',
      showCancelButton: false,
      showCloseButton: false,
      showConfirmButton: false,
      toast: true,
    });
  }

  callSwal(title:string, text: string, confirmButtonText:string="delete", callback: ()=>void, dismissed?: ()=>void ) {
    Swal.fire({
      title: title,
      text: text, 
      icon: "question",
      showConfirmButton: true,
      confirmButtonText: confirmButtonText,
      showCancelButton: true,
      cancelButtonText: 'Cancel'
    }).then(result => {
      if (result.isConfirmed) {
        callback();
      } else if (result.isDismissed) {
        dismissed?.();
      }
    });
  }

}

export type SweetAlertIcon = 'success' | 'error' | 'warning' | 'info' | 'question';
