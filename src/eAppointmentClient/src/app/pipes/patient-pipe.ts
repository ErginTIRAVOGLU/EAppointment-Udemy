import { Pipe, PipeTransform } from '@angular/core'; 
import { PatientModel } from '../models/patient.model';

@Pipe({
  name: 'patient'
})
export class PatientPipe implements PipeTransform {

  transform(value: PatientModel[], search: string): PatientModel[] {
    if(!search){
      return value;
    }
   
    const searchLower = search.toLocaleLowerCase('tr');
    const patients = value.filter( patient => 
      patient.fullName.toLocaleLowerCase('tr').includes(searchLower) ||
      patient.fullAddress.toLocaleLowerCase('tr').includes(searchLower) ||
      patient.city.toLocaleLowerCase('tr').includes(searchLower) ||
      patient.town.toLocaleLowerCase('tr').includes(searchLower) ||
      patient.identityNumber.toLocaleLowerCase('tr').includes(searchLower)
    );

    
    
    return patients;
  }

}
