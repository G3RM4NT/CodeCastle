import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClienteService } from '../../services/cliente.service';
import { ProductoService } from '../../services/producto.service';
import { VentaService } from '../../services/venta.service';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';

interface Producto {
  id: number;
  nombre: string;
  stock: number;
  precio: number;
}

interface DetalleVenta {
  productoId: number;
  nombre: string;
  cantidad: number;
  precioVenta: number;
}

interface Venta {
  clienteId: number;
  detalles: DetalleVenta[];
}

@Component({
  selector: 'app-ventas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ventas.component.html'
})
export class VentasComponent implements OnInit {

  clientes: any[] = [];
  productos: Producto[] = [];

  venta: Venta = {
    clienteId: 0,
    detalles: []
  };

  detalle: DetalleVenta = {
    productoId: 0,
    nombre: '',
    cantidad: 1,
    precioVenta: 0
  };

  constructor(
    private clienteService: ClienteService,
    private productoService: ProductoService,
    private ventaService: VentaService,
    private auth: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.getClientes();
    this.getProductos();
  }

  getClientes() {
    this.clienteService.getAll().subscribe({
      next: (res: any) => {
        this.clientes = res;
        this.cdr.detectChanges();
      },
      error: (err: HttpErrorResponse) => console.error('Error clientes:', err.message)
    });
  }

  getProductos() {
    this.productoService.getAll().subscribe({
      next: (res: any) => {
        this.productos = res;
        this.cdr.detectChanges();
      },
      error: (err: HttpErrorResponse) => console.error('Error productos:', err.message)
    });
  }

  trackById(index: number, item: any) {
    return item.id;
  }

  seleccionarProducto() {
    const idSeleccionado = Number(this.detalle.productoId);
    const prod = this.productos.find(p => p.id === idSeleccionado);

    if (prod) {
      this.detalle.nombre = prod.nombre;
      this.detalle.precioVenta = (prod as any).precioUnitario || (prod as any).precio || (prod as any).PrecioUnitario || 0;
      console.log('Producto encontrado:', prod);
    } else {
      this.detalle.precioVenta = 0;
    }
  }

  agregarProducto() {
    const idSeleccionado = Number(this.detalle.productoId);
    const prodOriginal = this.productos.find(p => p.id === idSeleccionado);

    if (!prodOriginal) {
      alert("Por favor, seleccione un producto de la lista.");
      return;
    }

    if (this.detalle.cantidad <= 0) {
      alert("La cantidad debe ser mayor a 0.");
      return;
    }

    const existente = this.venta.detalles.find(d => d.productoId === idSeleccionado);
    const cantidadTotal = existente ? (existente.cantidad + this.detalle.cantidad) : this.detalle.cantidad;

    if (cantidadTotal > prodOriginal.stock) {
      alert(`No puedes agregar esa cantidad. Stock disponible: ${prodOriginal.stock}. En tabla ya tienes: ${existente ? existente.cantidad : 0}`);
      return;
    }

    if (existente) {
      existente.cantidad = cantidadTotal;
    } else {
      this.venta.detalles.push({ ...this.detalle, productoId: idSeleccionado });
    }

    this.detalle = { productoId: 0, nombre: '', cantidad: 1, precioVenta: 0 };
  }

  eliminarDetalle(index: number) {
    this.venta.detalles.splice(index, 1);
  }

  total(): number {
    return this.venta.detalles.reduce((sum, d) => sum + (d.cantidad * d.precioVenta), 0);
  }

  guardarVenta() {
    if (this.venta.detalles.length === 0) {
      alert("Agregue productos a la venta");
      return;
    }

    const token = this.auth.getToken();
    let userId = 3;

    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        userId = payload.nameid || payload.sub || 3;
      } catch (e) {
        console.error("Error al decodificar token", e);
      }
    }

    const dataEnviar = {
      ClienteId: Number(this.venta.clienteId),
      UsuarioId: Number(userId),
      Detalles: this.venta.detalles.map(d => ({
        ProductoId: Number(d.productoId),
        Cantidad: Number(d.cantidad),
        PrecioVenta: Number(d.precioVenta)
      }))
    };

    this.ventaService.crearVenta(dataEnviar).subscribe({
      next: (res) => {
        alert('¡Venta registrada con éxito!');
        this.venta = { clienteId: 0, detalles: [] };
        this.getProductos();
      },
      error: (err) => {
        alert("Error: " + (err.error?.message || "No se pudo guardar la venta"));
      }
    });
  }
}