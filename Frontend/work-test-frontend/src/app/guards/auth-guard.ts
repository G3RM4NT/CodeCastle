import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {

  const token = localStorage.getItem('token');

  if (token) {
    return true;
  }

  // 🔥 Si no hay token → redirige
  const router = new Router();
  router.navigate(['/login']);
  return false;
};