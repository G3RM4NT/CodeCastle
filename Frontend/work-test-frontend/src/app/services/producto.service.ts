import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../enviroments/enviroment';

@Injectable({ providedIn: 'root' })
export class ProductoService {

  private api = `${environment.apiUrl}/producto`;

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get(this.api);
  }

  create(data: any) {
    return this.http.post(this.api, data);
  }

  update(id: number, data: any) {
    return this.http.put(`${this.api}/${id}`, data);
  }

  delete(id: number) {
    return this.http.delete(`${this.api}/${id}`);
  }

  getById(id: number) {
  return this.http.get(`${environment.apiUrl}/producto/${id}`);
}
}