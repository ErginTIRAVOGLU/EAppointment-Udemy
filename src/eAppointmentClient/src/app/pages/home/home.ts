import { Component, inject, OnInit, signal, computed, ViewChild, ElementRef } from '@angular/core';
import { DepartmentModel, initialDepartmentModel } from '../../models/department.model';
import { HttpService } from '../../services/http-service';
import { FormsModule, NgForm } from '@angular/forms';
import { DoctorModel } from '../../models/doctor.model';
import { AppointmentModel, initialAppointmentModel } from '../../models/appointment.model';
import { DatePipe } from '@angular/common';
import { AppointmentPipe } from '../../pipes/appointment-pipe';
import { FormValidateDirective } from 'form-validate-angular';
import { SwalService } from '../../services/swal-service';
import { CreateAppointmentModel, initialCreateAppointmentModel } from '../../models/create-appointment.model';
import { initialUpdateAppointmentModel, UpdateAppointmentModel } from '../../models/update-appointment.model';


@Component({
  selector: 'app-home',
  imports: [FormsModule,DatePipe,FormValidateDirective,AppointmentPipe],
  templateUrl: './home.html',
  styleUrl: './home.css',
  providers: [DatePipe]
})
export class Home implements OnInit {
  readonly #http = inject(HttpService);
  readonly #swal = inject(SwalService);
  readonly date = inject(DatePipe);

  readonly departments = signal<DepartmentModel[]>([]);
  readonly doctors = signal<DoctorModel[]>([]);
  readonly appointments = signal<AppointmentModel[]>([]);
  readonly search = signal<string>('');

  readonly selectedDepartmentValue = signal<number>(0);
  readonly selectedDoctorId = signal<string>('0');

  readonly appointmentCreateFormModel = signal<CreateAppointmentModel>(initialCreateAppointmentModel);
  readonly appointmentUpdateFormModel = signal<UpdateAppointmentModel>(initialUpdateAppointmentModel);
  @ViewChild('addModalCloseButton') readonly addModalCloseButton: ElementRef<HTMLButtonElement> | undefined;
  @ViewChild('updateModalCloseButton') readonly updateModalCloseButton: ElementRef<HTMLButtonElement> | undefined;

  // Reactive computed signal for showing doctor select
  readonly showDoctorSelect = computed(() => this.selectedDepartmentValue() !== 0);

  // Signal to check if patient is found (for disabling firstName input)
  readonly isPatientFound = signal<boolean>(true);

  ngOnInit(): void {
 
    this.GetAllDepartments();
    
  }

  onDepartmentChange(value: number): void {
    this.selectedDepartmentValue.set(value);
    // Reset doctor selection when department changes
    this.GetAllDoctors(this.selectedDepartmentValue());
    this.selectedDoctorId.set('0');
    
  }
  onDoctorChange(id: string): void {
    this.selectedDoctorId.set(id);
    this.GetAllAppointmentsByDoctorId(id);
  }

  private GetAllDepartments(): void {
    this.#http.get<DepartmentModel[]>('departments', (response) => {
        this.departments.set(response.data ?? []);
      },
      (error) => {
        console.error('Error fetching departments:', error);
      }
    );
  }

  private GetAllDoctors(departmantId: number): void {
    if(departmantId > 0) {
      this.#http.get<DoctorModel[]>(`doctors/departments/${departmantId}`, (response) => {
          this.doctors.set(response.data ?? []);
          this.selectedDoctorId.set('0');
        },
        (error) => {
          console.error('Error fetching doctors:', error);
        }
      );
    }
    else {
      this.doctors.set([]);
      this.selectedDoctorId.set('0');
    }
  }

  private GetAllAppointmentsByDoctorId(doctorId: string): void {
    this.#http.get<AppointmentModel[]>(`appointments/doctor/${doctorId}`, (response) => {
         
        this.appointments.set(response.data ?? []);
      },
      (error) => {
        console.error('Error fetching appointments:', error);
      }
    );
  }

  add(form:NgForm) {
    if (form.valid) {
      this.#http.post<string>('appointments', this.appointmentCreateFormModel(), (res)=> {
        this.#swal.callToast('Appointment added successfully','success',3000);
        this.GetAllAppointmentsByDoctorId(this.selectedDoctorId());
        this.appointmentCreateFormModel.set(initialCreateAppointmentModel);
        form.resetForm();
        this.addModalCloseButton?.nativeElement.click();
        this.isPatientFound.set(false);
      },
      (error) => {
        console.error('Error adding appointment:', error);
      });
    }
  }


  update(form:NgForm) {
      if (form.valid) {
      this.#http.put<string>('appointments', this.appointmentUpdateFormModel(), (res)=> {
        this.#swal.callToast(res.data!,'success',3000);
        this.GetAllAppointmentsByDoctorId(this.selectedDoctorId());
        this.appointmentUpdateFormModel.set(initialUpdateAppointmentModel);
        form.resetForm();
        this.updateModalCloseButton?.nativeElement.click();
      },
      (error) => {
        console.error('Error adding appointment:', error);
      });
    }
  }

 
  onAppointmentCreateModalOpen(e:any) {
    e.cancel = false;
   
    const appointmentCreateFormModel = signal<CreateAppointmentModel>(initialCreateAppointmentModel);
    appointmentCreateFormModel.set({
      startDate: this.date.transform(e.startDate, 'yyyy-MM-ddTHH:mm:ss') ?? '', 
      endDate: this.date.transform(e.endDate, 'yyyy-MM-ddTHH:mm:ss') ?? '',
      doctorId: this.selectedDoctorId(), 
      patientId: '', 
      FirstName: '', 
      LastName: '', 
      IdentityNumber: '', 
      City: '', 
      Town: '', 
      FullAddress: ''});
    this.isPatientFound.set(false);
  }

  getPatient(event:any) {
   if(event.key === 'Enter' || event.key === 'Tab') {
      event.preventDefault();
       const identityNumber = this.appointmentCreateFormModel().IdentityNumber;
    if(identityNumber.length >3) {
      this.#http.get<any>(`patients/identityNumber/${identityNumber}`, (response) => {
          const patient = response.data;
          if(patient) {
            this.appointmentCreateFormModel.update(current => ({
              ...current, 
              patientId: patient.id,
              FirstName: patient.firstName,
              LastName: patient.lastName,
              City: patient.city,
              Town: patient.town,
              FullAddress: patient.fullAddress,
              doctorId: this.selectedDoctorId(),

            }));
            this.isPatientFound.set(true);
          } else {
            this.#swal.callToast('Patient not found','error',3000);
            this.appointmentCreateFormModel.update( current => ({
              ...current, 
              patientId: '',
              FirstName: '',
              LastName: '',
              City: '',
              Town: '',
              FullAddress: '',
              doctorId: this.selectedDoctorId(),
            }));
            this.isPatientFound.set(false);

          }
        },
        (error) => {
          if(error.status === 404)
          {
            this.#swal.callToast('Patient not found','error',3000);
            this.appointmentCreateFormModel.update( current => ({
              ...current, 
              patientId: '',
              FirstName: '',
              LastName: '',
              City: '',
              Town: '',
              FullAddress: '',
              doctorId: this.selectedDoctorId(),
            }));
            this.isPatientFound.set(false);
          }
        });
      }
    }  
  }
  
  openAppointmentDetailModal(appointment:AppointmentModel) {
    
    this.#http.get<UpdateAppointmentModel>(`appointments/${appointment.id}`, (response) => {
   
        const appointmentDetail = response.data;
        if(appointmentDetail) {
          this.appointmentUpdateFormModel.set(appointmentDetail);
        }
      },
      (error) => {
        console.error('Error fetching appointment details:', error);
      }
    );
    
  }

  openAppointmentEditModal(id:string) {
    this.#http.get<UpdateAppointmentModel>(`appointments/${id}`, (response) => {
      
        const appointmentDetail = response.data;
        if(appointmentDetail) {
          this.appointmentUpdateFormModel.set(appointmentDetail);
        }
      },
      (error) => {
        console.error('Error fetching appointment details:', error);
      }
    );
  }
  deleteAppointment(appointmentId:string, title:string) {
    this.#swal.callSwal("Delete Appointment", `Are you sure you want to delete the appointment: ${title}?`, "Delete", () => {
 
      this.#http.delete<string>(`appointments/${appointmentId}`, (res) => {
        this.#swal.callToast(res.data!,'success',3000);
        this.GetAllAppointmentsByDoctorId(this.selectedDoctorId());
      },
      (error) => {
        console.error('Error deleting appointment:', error);
      });
    });
  }

}
