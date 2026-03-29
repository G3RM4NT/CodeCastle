import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../enviroments/enviroment';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class VentaService {

  constructor(private http: HttpClient) {}

  // Para la pantalla de REPORTES (Listar)
  getAll(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/Venta`);
  }

  // Para la pantalla de NUEVA VENTA (Guardar)
  crearVenta(data: any): Observable<any> {
    // Agregamos responseType text por si el backend responde un string simple
    return this.http.post(`${environment.apiUrl}/Venta`, data, { responseType: 'text' });
  }
}