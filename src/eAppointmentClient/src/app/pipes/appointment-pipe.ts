import { Pipe, PipeTransform } from '@angular/core';
import { DoctorModel } from '../models/doctor.model';
import { AppointmentModel } from '../models/appointment.model';

@Pipe({
  name: 'appointment'
})
export class AppointmentPipe implements PipeTransform {

  transform(value: AppointmentModel[], search: string): AppointmentModel[] {
    if(!search){
      return value;
    }
   
    const appointments = value.filter( appointment => 
      appointment.patientFullName.toLowerCase().includes(search.toLowerCase()) ||
      appointment.patientIdentityNumber!.toLowerCase().includes(search.toLowerCase())
    );

    
    return appointments;

  }

}
