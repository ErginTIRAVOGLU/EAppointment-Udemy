import { Pipe, PipeTransform } from '@angular/core';
import { UserModel } from '../models/user.model';

@Pipe({
  name: 'user'
})
export class UserPipe implements PipeTransform {

  transform(value: UserModel[], search: string): UserModel[] {
      if(!search){
        return value;
      }
     
      const searchLower = search.toLocaleLowerCase('tr');
      const users = value.filter( user => 
        user.fullName.toLocaleLowerCase('tr').includes(searchLower) ||
        user.userName.toLocaleLowerCase('tr').includes(searchLower) ||
        user.email.toLocaleLowerCase('tr').includes(searchLower)
      );
  
      
      
      return users;
    }

}
