import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../enviroments/enviroment';

@Injectable({ providedIn: 'root' })
export class AuthService {

  constructor(private http: HttpClient) {}

  login(data: any) {
    return this.http.post(`${environment.apiUrl}/auth/login`, data);
  }


  register(data: any) {
    return this.http.post(`${environment.apiUrl}/auth/register`, data);
  }

  saveToken(token: string) {
    localStorage.setItem('token', token);
  }

  getToken() {
    return localStorage.getItem('token');
  }

  isLogged() {
    return !!this.getToken();
  }

  logout() {
    localStorage.removeItem('token');
  }
}