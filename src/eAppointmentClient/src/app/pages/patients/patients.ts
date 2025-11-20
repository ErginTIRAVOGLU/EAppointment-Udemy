import { Component, ElementRef, inject, OnInit, signal, ViewChild } from '@angular/core';
import { HttpService } from '../../services/http-service';
import { SwalService } from '../../services/swal-service'; 
import { initialPatientModel, PatientModel } from '../../models/patient.model';
import { DepartmentModel } from '../../models/department.model';
import { FormsModule, NgForm } from '@angular/forms';
import { PatientPipe } from '../../pipes/patient-pipe';
import { FormValidateDirective } from 'form-validate-angular';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-patients',
  imports: [RouterLink, FormsModule,FormValidateDirective,PatientPipe],
  templateUrl: './patients.html',
  styleUrl: './patients.css',
})
export class Patients implements OnInit {
 readonly #http = inject(HttpService);
  readonly #swal = inject(SwalService);
  readonly patients = signal<PatientModel[]>([]);
  readonly search = signal<string>('');
  readonly patientFormModel = signal<PatientModel>(initialPatientModel);
  readonly patientUpdateFormModel = signal<PatientModel>(initialPatientModel);

  
 
  @ViewChild('addModalCloseButton') readonly addModalCloseButton: ElementRef<HTMLButtonElement> | undefined;
  @ViewChild('updateModalCloseButton') readonly updateModalCloseButton: ElementRef<HTMLButtonElement> | undefined;

  getAllPatients() {
    this.#http.get<PatientModel[]>('patients', (response) => {
      this.patients.set(response.data ?? []);
      console.log('Patients fetched:', this.patients());
    },
    (error) => {
      this.#swal.callToast('Error fetching patients','error',3000);
    });
  }
 
  add(form:NgForm) {
    if (form.valid) {
      this.#http.post<string>('patients', this.patientFormModel(), (res)=> {
        this.#swal.callToast('Patient added successfully','success',3000);
        this.getAllPatients();
        this.patientFormModel.set(initialPatientModel);
        form.resetForm();
        this.addModalCloseButton?.nativeElement.click();
      },
      (error) => {
        console.error('Error adding patient:', error);
      });
    }
  }

  get(data:PatientModel) {
     
    this.patientUpdateFormModel.set({...data});
  }

  update(form:NgForm) {
     if (form.valid) {
      this.#http.put<string>('patients', this.patientUpdateFormModel(), (res)=> {
        this.#swal.callToast(res.data!,'success',3000);
        this.getAllPatients();
        this.patientUpdateFormModel.set(initialPatientModel);
        form.resetForm();
        this.updateModalCloseButton?.nativeElement.click();
      },
      (error) => {
        console.error('Error adding patient:', error);
      });
    }
  }

  delete(patientId:string, fullName:string) {
 
    this.#swal.callSwal("Delete Patient", `Deleting patient: ${fullName}`, "Delete", () => {
      this.#http.delete<string>(`patients/${patientId}`, (res) => {
        this.#swal.callToast(res.data!,'success',3000);
        this.getAllPatients();
      },
      (error) => {
        console.error('Error deleting patient:', error);
      });
    });
  }

  ngOnInit() {
     
    this.getAllPatients();
     
  }

}
