import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { Navbar } from './navbar/navbar';

@Component({
  selector: 'app-layouts',
  imports: [RouterOutlet, Navbar],
  templateUrl: './layouts.html',
  styleUrl: './layouts.css',
})
export class Layouts {

}
