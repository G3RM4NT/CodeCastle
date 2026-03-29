import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClienteService } from '../../services/cliente.service';
import { ProductoService } from '../../services/producto.service';
import { VentaService } from '../../services/venta.service'; // Asegúrate de tenerlo
import { HttpErrorResponse } from '@angular/common/http';

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
  templateUrl:'./ventas.component.html'
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
    private ventaService: VentaService
  ) {}

  ngOnInit() {
    this.getClientes();
    this.getProductos();
  }

  getClientes() {
    this.clienteService.getAll().subscribe({
      next: (res: any) => this.clientes = res,
      error: (err: HttpErrorResponse) => console.error('Error clientes:', err.message)
    });
  }

  getProductos() {
    this.productoService.getAll().subscribe({
      next: (res: any) => this.productos = res,
      error: (err: HttpErrorResponse) => console.error('Error productos:', err.message)
    });
  }

seleccionarProducto() {
  const idSeleccionado = Number(this.detalle.productoId);
  const prod = this.productos.find(p => p.id === idSeleccionado);

  if (prod) {
    this.detalle.nombre = prod.nombre;
    
    // 💡 INTENTO DE MAPEO DINÁMICO:
    // Probamos con 'precioUnitario' (estándar JSON), luego 'precio' (tu interfaz) 
    // y finalmente 'PrecioUnitario' (como está en SQL)
    this.detalle.precioVenta = (prod as any).precioUnitario || (prod as any).precio || (prod as any).PrecioUnitario || 0;

    console.log('Producto encontrado:', prod); // Revisa en la consola del navegador qué nombre trae
  } else {
    this.detalle.precioVenta = 0;
  }
}

agregarProducto() {
  const idSeleccionado = Number(this.detalle.productoId);
  const prodOriginal = this.productos.find(p => p.id === idSeleccionado);

  // 1. Validar que seleccionó algo
  if (!prodOriginal) {
    alert("Por favor, seleccione un producto de la lista.");
    return;
  }

  // 2. Validar números negativos o cero
  if (this.detalle.cantidad <= 0) {
    alert("La cantidad debe ser mayor a 0.");
    return;
  }

  // 3. VALIDACIÓN DE STOCK (El "OJO" de la prueba)
  // Buscamos si ya existe el producto en la tabla para sumar las cantidades
  const existente = this.venta.detalles.find(d => d.productoId === idSeleccionado);
  const cantidadTotal = existente ? (existente.cantidad + this.detalle.cantidad) : this.detalle.cantidad;

  if (cantidadTotal > prodOriginal.stock) {
    alert(`No puedes agregar esa cantidad. Stock disponible: ${prodOriginal.stock}. En tabla ya tienes: ${existente ? existente.cantidad : 0}`);
    return;
  }

  // 4. Si pasa las validaciones, agregamos o actualizamos
  if (existente) {
    existente.cantidad = cantidadTotal;
  } else {
    // Usamos spread operator para crear una copia y asegurar que el ID sea número
    this.venta.detalles.push({ ...this.detalle, productoId: idSeleccionado });
  }

  // 5. Limpiar el formulario de selección
  this.detalle = { productoId: 0, nombre: '', cantidad: 1, precioVenta: 0 };
}

  eliminarDetalle(index: number) {
    this.venta.detalles.splice(index, 1);
  }

  total(): number {
    return this.venta.detalles.reduce((sum, d) => sum + (d.cantidad * d.precioVenta), 0);
  }

  guardarVenta() {
    // 🚩 ADVERTENCIA CAMPOS OBLIGATORIOS
    if (Number(this.venta.clienteId) === 0 || this.venta.detalles.length === 0) {
      alert("Advertencia: completar campos obligatorios (Cliente y al menos un producto en tabla).");
      return;
    }

    // Convertir IDs a números para evitar errores en el Backend
    const dataEnviar = {
      clienteId: Number(this.venta.clienteId),
      detalles: this.venta.detalles
    };

    this.ventaService.crearVenta(dataEnviar).subscribe({
      next: (res) => {
        alert('¡Venta registrada con éxito y Stock actualizado!');
        this.venta = { clienteId: 0, detalles: [] };
        this.getProductos(); // Refrescar lista de productos para ver el nuevo stock
      },
      error: (err) => {
        alert('Error al guardar: ' + (err.error?.message || err.message));
      }
    });
  }
}