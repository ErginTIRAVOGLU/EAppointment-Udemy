import { Component, ElementRef, inject, OnInit, signal, viewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { initialLoginModel, LoginModel } from '../../models/login.model';
import { FormValidateDirective } from 'form-validate-angular';
import { HttpService } from '../../services/http-service';
import { LoginResponseModel } from '../../models/login-response.model';

@Component({
  selector: 'app-login',
  imports: [FormsModule, FormValidateDirective],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login implements OnInit {
  readonly #httpService = inject(HttpService);

  ngOnInit(): void {}
  constructor() {}
  readonly #router = inject(Router);
  readonly loginModel = signal<LoginModel>(initialLoginModel);

  passwordInput = viewChild<ElementRef<HTMLInputElement> | undefined>('passwordInput');

  showOrHidePassword() {
    const input = this.passwordInput();

    if (input === undefined) return;

    if (input.nativeElement.type === 'password') {
      input.nativeElement.type = 'text';
    } else {
      input.nativeElement.type = 'password';
    }
  }

  signIn(form: NgForm) {
    if (form.valid) {
      this.#httpService.post<LoginResponseModel>(
        'auth/login',
        this.loginModel(),
        (response) => {
          localStorage.setItem('token', response.data?.token || '');
          this.#router.navigateByUrl('/');
        },
        (err) => {
          console.error(err.error.message);
        }
      );
    }
  }
}
