import { Component, OnInit, ChangeDetectorRef } from '@angular/core'; // 1. Importar ChangeDetectorRef
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { VentaService } from '../../services/venta.service';

@Component({
  selector: 'app-reporte-ventas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: 'reportesventas.component.html' 
})
export class ReporteVentasComponent implements OnInit {
  ventasOriginales: any[] = [];
  ventasFiltradas: any[] = [];

  filtroNombre: string = '';
  fechaInicio: string = '';
  fechaFin: string = '';

  // 2. Inyectar en el constructor
  constructor(
    private ventaService: VentaService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.cargarReporte();
  }

  cargarReporte() {
    this.ventaService.getAll().subscribe({
      next: (res: any[]) => {
        // 3. Spread y detección instantánea
        this.ventasOriginales = [...res];
        this.ventasFiltradas = [...res];
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error:', err);
        alert('Error al conectar con la API');
      }
    });
  }

  aplicarFiltros() {
    if (!this.ventasOriginales) return;

    this.ventasFiltradas = this.ventasOriginales.filter(v => {
      // Manejo flexible de Mayúsculas/minúsculas para Cliente
      const clienteObj = v.Cliente || v.cliente;
      const nombreCliente = clienteObj?.Nombre?.toLowerCase() || clienteObj?.nombre?.toLowerCase() || '';
      const coincideNombre = nombreCliente.includes(this.filtroNombre.toLowerCase());

      const fechaRaw = v.Fecha || v.fecha;
      const fechaVenta = new Date(fechaRaw).setHours(0,0,0,0);
      
      let coincideFecha = true;
      if (this.fechaInicio) {
        const inicio = new Date(this.fechaInicio).setHours(0,0,0,0);
        if (fechaVenta < inicio) coincideFecha = false;
      }
      if (this.fechaFin) {
        const fin = new Date(this.fechaFin).setHours(23,59,59,999);
        if (fechaVenta > fin) coincideFecha = false;
      }
      return coincideNombre && coincideFecha;
    });
    this.cdr.detectChanges();
  }

  limpiarFiltros() {
    this.filtroNombre = '';
    this.fechaInicio = '';
    this.fechaFin = '';
    this.ventasFiltradas = [...this.ventasOriginales];
    this.cdr.detectChanges();
  }

  calcularTotalReporte(): number {
    return this.ventasFiltradas.reduce((total, v) => {
      // Manejo flexible de Detalles/detalles
      const detalles = v.VentaDetalles || v.detalles || [];
      const subtotalVenta = detalles.reduce((sub: number, d: any) => {
        const cant = d.Cantidad || d.cantidad || 0;
        const precio = d.PrecioVenta || d.precioVenta || 0;
        return sub + (cant * precio);
      }, 0);
      return total + subtotalVenta;
    }, 0);
  }
}