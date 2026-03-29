import { Component, OnInit } from '@angular/core';
import { ProductoService } from '../../services/producto.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-productos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './productos.component.html'
})
export class ProductosComponent implements OnInit {

  productos: any[] = [];


  producto: any = {};

  editando = false;

  constructor(private service: ProductoService) {}

  ngOnInit() {
    this.getProductos();
  }

  getProductos() {
    this.service.getAll().subscribe((res: any) => {
      this.productos = res;
    });
  }

 guardar() {

  // 🔥 VALIDACIÓN
  if (!this.producto.nombre || !this.producto.precioUnitario) {
    alert('Completa los campos obligatorios');
    return;
  }

  if (this.editando) {
    this.service.update(this.producto.id, this.producto)
      .subscribe(() => {
        this.getProductos();
        this.reset();
      });
  } else {
    this.service.create(this.producto)
      .subscribe(() => {
        this.getProductos();
        this.reset();
      });
  }
}

 editar(p: any) {

  if (p.tieneCompras) {
    alert('No puedes editar este producto porque tiene compras asociadas');
    return;
  }

  this.producto = { ...p };
  this.editando = true;
}

eliminar(id: number) {
  if (confirm('¿Eliminar producto?')) {
    this.service.delete(id).subscribe({
      next: () => {
        this.getProductos();
        this.reset();
      },
      error: (err) => {
        console.error(err);
        alert('No puedes eliminar este producto porque tiene compras asociadas');
      }
    });
  }
}

  reset() {
    this.producto = {}; // 🔥 limpiar completamente
    this.editando = false;
  }
}