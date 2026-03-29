import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent implements OnInit {
  usuarioNombre: string = 'Usuario';
  usuarioRol: string = 'Sin Rol';

  constructor(
    private auth: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.obtenerDatosUsuario();
  }

  obtenerDatosUsuario() {
    const token = this.auth.getToken();
    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        
        // Extraer Nombre (ajusta la llave 'unique_name' según tu Backend)
        this.usuarioNombre = payload.unique_name || payload.name || payload.email || 'Usuario';
        
        // Extraer Rol usando tu método existente en AuthService
        this.usuarioRol = this.auth.getUserRole();
      } catch (e) {
        console.error('Error al decodificar token', e);
      }
    }
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }

  goToProductos() { this.router.navigate(['/productos']); }
  goToVentas() { this.router.navigate(['/ventas']); }
  goToCompras() { this.router.navigate(['/compras']); }
  goToReporteVenta() { this.router.navigate(['/reportev']); }
  goToReporteCompra() { this.router.navigate(['/reportec']); }
}