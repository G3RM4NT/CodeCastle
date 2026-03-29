import { Component, OnInit, ChangeDetectorRef } from '@angular/core'; // 1. Importar ChangeDetectorRef
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

  // 2. Inyectar ChangeDetectorRef en el constructor
  constructor(
    private service: ProductoService,
    private cdr: ChangeDetectorRef 
  ) {}

  ngOnInit() {
    this.getProductos();
  }

  getProductos() {
    this.service.getAll().subscribe((res: any) => {
      // 3. Usar el operador [...] para crear una nueva referencia de memoria
      this.productos = [...res]; 
      
      // 4. Forzar a Angular a que pinte los cambios AHORA mismo
      this.cdr.detectChanges(); 
    });
  }

  guardar() {
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
    this.producto = {};
    this.editando = false;
  }
}