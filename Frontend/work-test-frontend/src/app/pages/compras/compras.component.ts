import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CompraService } from '../../services/compra.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../enviroments/enviroment';
import { ProveedorService } from '../../services/proveedor.service';

@Component({
  selector: 'app-compras',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './compras.component.html'
})
export class ComprasComponent implements OnInit {
  productos: any[] = [];
  proveedores: any[] = [];

  idProveedor: number = 0;
  idProducto: number = 0;
  cantidad: number = 1;
  precioCosto: number = 0;

  detalleCompra: any[] = [];
  totalCompra: number = 0;

  constructor(
    private http: HttpClient, 
    private compraService: CompraService,
    private proveedorService: ProveedorService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.cargarProductos();
    this.cargarProveedores();
  }

  cargarProductos() {
    this.http.get<any[]>(`${environment.apiUrl}/Producto`).subscribe({
      next: (res) => {
        this.productos = res;
        this.cdr.detectChanges();
      },
      error: (err) => console.error("Error al cargar productos:", err)
    });
  }

  cargarProveedores() {
    this.proveedorService.getAll().subscribe({
      next: (res) => {
        this.proveedores = res;
        this.cdr.detectChanges();
      },
      error: (err) => console.error("Error al cargar proveedores:", err)
    });
  }

  trackById(index: number, item: any) {
    return item.id;
  }

  agregarProducto() {
    const producto = this.productos.find(p => p.id === Number(this.idProducto));
    if (!producto || this.cantidad <= 0) return;

    this.detalleCompra.push({
      productoId: producto.id,
      nombreProducto: producto.nombre,
      cantidad: this.cantidad,
      precioCosto: this.precioCosto,
      subtotal: this.cantidad * this.precioCosto
    });

    this.calcularTotal();
    this.idProducto = 0;
    this.cantidad = 1;
    this.precioCosto = 0;
  }

  eliminarProducto(index: number) {
    this.detalleCompra.splice(index, 1);
    this.calcularTotal();
  }

  calcularTotal() {
    this.totalCompra = this.detalleCompra.reduce((sum, item) => sum + item.subtotal, 0);
  }

  guardarCompra() {
    if (this.idProveedor === 0 || this.detalleCompra.length === 0) {
      alert("Selecciona un proveedor y productos");
      return;
    }

    const compraData = {
      proveedorId: Number(this.idProveedor),
      total: this.totalCompra,
      detalles: this.detalleCompra.map(d => ({
        productoId: d.productoId,
        cantidad: d.cantidad,
        precioCosto: d.precioCosto
      }))
    };

    this.compraService.crearCompra(compraData).subscribe({
      next: (response) => {
        alert("¡Compra exitosa! El stock se ha actualizado.");
        this.limpiarFormulario();
        this.cargarProductos(); 
      },
      error: (err) => {
        console.error("Error detallado:", err);
        alert("Error al procesar la compra en el servidor. Revisa la consola.");
      }
    });
  }

  limpiarFormulario() {
    this.detalleCompra = [];
    this.idProveedor = 0;
    this.totalCompra = 0;
    this.idProducto = 0;
    this.cantidad = 1;
    this.precioCosto = 0;
  }
}