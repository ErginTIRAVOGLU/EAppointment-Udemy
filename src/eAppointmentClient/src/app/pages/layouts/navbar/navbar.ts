import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../services/auth-service';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  readonly #router = inject(Router);
  readonly authService = inject(AuthService)
  signOut() {
    localStorage.removeItem('token');
    this.#router.navigateByUrl('/login');
  }

}
