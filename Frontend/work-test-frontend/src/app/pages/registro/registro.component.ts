import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './registro.component.html'
})
export class RegistroComponent {
  // Objeto que coincide con tu DTO del Backend
  usuario = {
    nombre: '',
    email: '',
    password: '',
    rol: 'Vendedor' // Rol por defecto
  };

  constructor(private authService: AuthService, private router: Router) {}

  ejecutarRegistro() {
    if (!this.usuario.nombre || !this.usuario.email || !this.usuario.password) {
      alert('Por favor, completa todos los campos');
      return;
    }

    this.authService.register(this.usuario).subscribe({
      next: (res) => {
        alert('¡Registro exitoso! Ahora puedes iniciar sesión.');
        this.router.navigate(['/login']); // Redirigir al login
      },
      error: (err) => {
        console.error(err);
        alert('Error al registrar: ' + (err.error || 'Intenta con otro correo'));
      }
    });
  }
}