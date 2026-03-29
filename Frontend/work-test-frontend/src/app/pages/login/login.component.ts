import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.css'
})
export class LoginComponent {
  email = '';
  password = '';

  constructor(
    private auth: AuthService,
    private router: Router
  ) {}

  login() {
    const data = {
      email: this.email,
      password: this.password
    };

    this.auth.login(data).subscribe({
      next: (res: any) => {
        console.log('Respuesta:', res);

        // ✅ Validar que venga token
        if (res?.token) {
          this.auth.saveToken(res.token);

          // 🔥 Redirección (puedes cambiar a dashboard si quieres)
          this.router.navigate(['/dashboard']);
        } else {
          alert('No se recibió token');
        }
      },
      error: (err) => {
        console.error('Error login:', err);
        alert('Credenciales incorrectas');
      }
    });
  }
}