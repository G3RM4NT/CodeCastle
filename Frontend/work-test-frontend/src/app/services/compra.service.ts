import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../enviroments/enviroment';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CompraService {

  constructor(private http: HttpClient) {}

  // Para registrar una nueva compra
  crearCompra(data: any): Observable<any> {
    return this.http.post(`${environment.apiUrl}/Compra`, data, { responseType: 'text' });
  }

  // Para el futuro reporte de compras
  getAll(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/Compra`);
  }
}