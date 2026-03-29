import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service'; // Asegúrate de que la ruta sea correcta

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const token = localStorage.getItem('token');

  // 1. Verificación de Token (Lo que ya tenías)
  if (!token) {
    router.navigate(['/login']);
    return false;
  }

  // 2. Verificación de Roles y Alerta de Permisos
  const userRole = authService.getUserRole();
  const expectedRoles = route.data?.['roles'] as Array<string>;

  // Si la ruta pide un rol específico (como Administrador) y el usuario NO lo tiene
  if (expectedRoles && !expectedRoles.includes(userRole)) {
    alert(' No tienes permisos para acceder a este módulo.');
    router.navigate(['/dashboard']); 
    return false;
  }

  return true;
};