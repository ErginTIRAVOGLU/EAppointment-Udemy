import { inject } from '@angular/core';
import { Routes } from '@angular/router';
import { AuthService } from './services/auth-service';

export const routes: Routes = [
    {
        path: 'login',
        loadComponent: () => import('./pages/login/login').then(m => m.Login)
    },
    {
        path: '',
        loadComponent: () => import('./pages/layouts/layouts').then(m => m.Layouts),
        canActivateChild: [()=> inject(AuthService).isAuthenticated()],
        children: [
            {
                path: '',
                loadComponent: () => import('./pages/home/home').then(m => m.Home)
            },
            {
                path: 'doctors',
                loadComponent: () => import('./pages/doctors/doctors').then(m => m.Doctors)
            },
            {
                path: 'patients',
                loadComponent: () => import('./pages/patients/patients').then(m => m.Patients)
            },
            {
                path: 'users',
                loadComponent: () => import('./pages/users/users').then(m => m.Users)
            }
        ]
    },
    {
        path: '**',
        loadComponent: () => import('./pages/not-found/not-found').then(m => m.NotFound)
    }
];
