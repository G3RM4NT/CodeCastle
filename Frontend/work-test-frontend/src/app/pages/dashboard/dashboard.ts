import { Component } from '@angular/core';
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
export class DashboardComponent {

  constructor(
    private auth: AuthService,
    private router: Router
  ) {}

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }

  goToProductos() {
    this.router.navigate(['/productos']);
  }

  goToVentas() {
    this.router.navigate(['/ventas']);
  }

  goToCompras() {
    this.router.navigate(['/compras']);
  }

  goToReporteVenta() {
    this.router.navigate(['/reportev']);
  }

   goToReporteCompra() {
    this.router.navigate(['/reportec']);
  }
}