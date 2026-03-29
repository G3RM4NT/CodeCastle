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
  { path: 'login', component: LoginComponent },

  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },

  { path: 'productos', component: ProductosComponent },

  { path: 'ventas', component: VentasComponent },

  { path: 'compras', component: ComprasComponent },

  {
    path: 'reportev',
    loadComponent: () => import('./pages/reportes/reportes.venta').then(m => m.ReporteVentasComponent),
  },

  { path: 'reportec', component: ReporteComprasComponent },

  { path: 'registro', component: RegistroComponent }


];