import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { DashboardComponent } from './pages/dashboard/dashboard';
import { authGuard } from './guards/auth-guard';
import { ProductosComponent } from './pages/productos/productos';
import { VentasComponent } from './pages/ventas/ventas.component';
import { ComprasComponent } from './pages/compras/compras.component';
import { ReporteComprasComponent } from './pages/reportes/reporte.compra';
import { RegistroComponent } from './pages/registro/registro.component';

export const routes: Routes = [
  { path: '', redirectTo: 'registro', pathMatch: 'full' }, 

  { path: 'login', component: LoginComponent },
  { path: 'registro', component: RegistroComponent },
  
  // Rutas accesibles por ambos (Admin y Vendedor)
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },
  { path: 'productos', component: ProductosComponent, canActivate: [authGuard] },
  { path: 'ventas', component: VentasComponent, canActivate: [authGuard] },
  {
    path: 'reportev',
    loadComponent: () => import('./pages/reportes/reportes.venta').then(m => m.ReporteVentasComponent),
    canActivate: [authGuard]
  },

  // 🛡️ Rutas protegidas SOLO para Administrador
  { 
    path: 'compras', 
    component: ComprasComponent, 
    canActivate: [authGuard], 
    data: { roles: ['Administrador'] } 
  },
  { 
    path: 'reportec', 
    component: ReporteComprasComponent, 
    canActivate: [authGuard], 
    data: { roles: ['Administrador'] } 
  }
];