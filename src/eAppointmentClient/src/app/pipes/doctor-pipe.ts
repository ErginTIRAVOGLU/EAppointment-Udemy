import { Pipe, PipeTransform } from '@angular/core';
import { DoctorModel } from '../models/doctor.model';

@Pipe({
  name: 'doctor'
})
export class DoctorPipe implements PipeTransform {

  transform(value: DoctorModel[], search: string): DoctorModel[] {
    if(!search){
      return value;
    }
   
    const doctors = value.filter( doctor => 
      doctor.fullName.toLowerCase().includes(search.toLowerCase()) ||
      doctor.departmentName!.toLowerCase().includes(search.toLowerCase())
    );

    
    return doctors;

  }

}
