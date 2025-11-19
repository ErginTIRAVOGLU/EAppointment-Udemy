 import { Component, ElementRef, inject, OnInit, signal, ViewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DoctorModel, initialDoctorModel } from '../../models/doctor.model';
import { HttpService } from '../../services/http-service';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from "@angular/forms";
import { DepartmentModel } from '../../models/department.model';
import { FormValidateDirective } from 'form-validate-angular';
import { SwalService } from '../../services/swal-service';

@Component({
  selector: 'app-doctors',
  imports: [RouterLink, FormsModule,FormValidateDirective],
  templateUrl: './doctors.html',
  styleUrl: './doctors.css',
})
export class Doctors implements OnInit {
  readonly #http = inject(HttpService);
  readonly #swal = inject(SwalService);
  readonly doctors = signal<DoctorModel[]>([]);
  readonly doctorFormModel = signal<DoctorModel>(initialDoctorModel);
  readonly doctorUpdateFormModel = signal<DoctorModel>(initialDoctorModel);

  
  readonly departments = signal<DepartmentModel[]>([]);
  @ViewChild('addModalCloseButton') readonly addModalCloseButton: ElementRef<HTMLButtonElement> | undefined;
  @ViewChild('updateModalCloseButton') readonly updateModalCloseButton: ElementRef<HTMLButtonElement> | undefined;

  getAllDoctors() {
    this.#http.get<DoctorModel[]>('doctors', 
    (response) => {
      this.doctors.set(response.data ?? []);
    },
    (error) => {
      console.error('Error fetching doctors:', error);
    });
  }

  getAllDepartments() {
    this.#http.get<DepartmentModel[]>('departments',  
    (response) => {
      this.departments.set(response.data ?? []);
    },
    (error) => {
      console.error('Error fetching departments:', error);
    });
  }

  add(form:NgForm) {
    if (form.valid) {
      this.#http.post<string>('doctors', this.doctorFormModel(), (res)=> {
        this.#swal.callToast('Doctor added successfully','success',3000);
        this.getAllDoctors();
        this.doctorFormModel.set(initialDoctorModel);
        form.resetForm();
        this.addModalCloseButton?.nativeElement.click();
      },
      (error) => {
        console.error('Error adding doctor:', error);
      });
    }
  }

  get(data:DoctorModel) {
     
    this.doctorUpdateFormModel.set({...data});
  }

  update(form:NgForm) {
     if (form.valid) {
      this.#http.put<string>('doctors', this.doctorUpdateFormModel(), (res)=> {
        this.#swal.callToast(res.data!,'success',3000);
        this.getAllDoctors();
        this.doctorUpdateFormModel.set(initialDoctorModel);
        form.resetForm();
        this.updateModalCloseButton?.nativeElement.click();
      },
      (error) => {
        console.error('Error adding doctor:', error);
      });
    }
  }

  delete(doctorId:string, fullName:string) {
 
    this.#swal.callSwal("Delete Doctor", `Deleting doctor: ${fullName}`, "Delete", () => {
      this.#http.delete<string>(`doctors/${doctorId}`, (res) => {
        this.#swal.callToast(res.data!,'success',3000);
        this.getAllDoctors();
      },
      (error) => {
        console.error('Error deleting doctor:', error);
      });
    });
  }

  ngOnInit() {
     
    this.getAllDoctors();
    this.getAllDepartments();
  }

  
}
