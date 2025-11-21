import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { HttpService } from '../../services/http-service';
import { SwalService } from '../../services/swal-service';
import { initialUserModel, UserModel } from '../../models/user.model';
import { FormsModule, NgForm } from '@angular/forms';
import { UserPipe } from '../../pipes/user-pipe';
import { FormValidateDirective } from 'form-validate-angular';
import { RouterLink } from '@angular/router';
import { RoleModel } from '../../models/role.model';

@Component({
  selector: 'app-users',
  imports: [RouterLink, FormsModule,FormValidateDirective,UserPipe],
  templateUrl: './users.html',
  styleUrl: './users.css',
})
export class Users {
readonly #http = inject(HttpService);
  readonly #swal = inject(SwalService);
  readonly users = signal<UserModel[]>([]);
  readonly search = signal<string>('');
  readonly roles =signal<RoleModel[]>([]);
  readonly userFormModel = signal<UserModel>(initialUserModel);
  readonly userUpdateFormModel = signal<UserModel>(initialUserModel);

  
 
  @ViewChild('addModalCloseButton') readonly addModalCloseButton: ElementRef<HTMLButtonElement> | undefined;
  @ViewChild('updateModalCloseButton') readonly updateModalCloseButton: ElementRef<HTMLButtonElement> | undefined;

  getAllUsers() {
    this.#http.get<UserModel[]>('users', (response) => {
      this.users.set(response.data ?? []);
      console.log('Users fetched:', this.users());
    },
    (error) => {
      this.#swal.callToast('Error fetching users','error',3000);
    });
  }

  getAllRoles() {
    this.#http.get<RoleModel[]>('roles', (response) => {
      this.roles.set(response.data ?? []);
      console.log('Roles fetched:', this.roles());
    },
    (error) => {
      this.#swal.callToast('Error fetching roles','error',3000);
    });
  }

  add(form:NgForm) {
    if (form.valid) {
      console.log('Adding user:', this.userFormModel());
      this.#http.post<string>('users', this.userFormModel(), (res)=> {
        this.#swal.callToast('User added successfully','success',3000);
        this.getAllUsers();
        this.userFormModel.set(initialUserModel);
        form.resetForm();
        this.addModalCloseButton?.nativeElement.click();
      },
      (error) => {
        console.error('Error adding user:', error);
      });
    }
  }

  get(data:UserModel) {
    this.userUpdateFormModel.set({...data});
    
    // Get user roles
    this.#http.get<string[]>(`users/${data.id}/roles`, (response) => {
      const roleIds = response.data ?? [];
      this.userUpdateFormModel.update(current => ({
        ...current,
        roleIds: roleIds
      }));
      console.log('User roles fetched:', roleIds);
    },
    (error) => {
      console.error('Error fetching user roles:', error);
    });
  }

  update(form:NgForm) {
     if (form.valid) {
      this.#http.put<string>(`users/${this.userUpdateFormModel().id}`, this.userUpdateFormModel(), (res)=> {
        this.#swal.callToast(res.data!,'success',3000);
        this.getAllUsers();
        this.userUpdateFormModel.set(initialUserModel);
        form.resetForm();
        this.updateModalCloseButton?.nativeElement.click();
      },
      (error) => {
        console.error('Error adding user:', error);
      });
    }
  }

  delete(userId:string, fullName:string) {
 
    this.#swal.callSwal("Delete User", `Deleting user: ${fullName}`, "Delete", () => {
      this.#http.delete<string>(`users/${userId}`, (res) => {
        this.#swal.callToast(res.data!,'success',3000);
        this.getAllUsers();
      },
      (error) => {
        console.error('Error deleting user:', error);
      });
    });
  }

  ngOnInit() {
     
    this.getAllUsers();
    this.getAllRoles();
     
  }
}
