import { Component, OnInit } from '@angular/core';
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

  // Filtros inicializados
  filtroNombre: string = '';
  fechaInicio: string = '';
  fechaFin: string = '';

  constructor(private ventaService: VentaService) {}

  ngOnInit() {
    this.cargarReporte();
  }

  cargarReporte() {
    this.ventaService.getAll().subscribe({
      next: (res: any[]) => {
        this.ventasOriginales = res;
        this.ventasFiltradas = res; // Mostramos todas al inicio
        console.log('Datos cargados:', res);
      },
      error: (err) => {
        console.error('Error:', err);
        alert('Error al conectar con la API');
      }
    });
  }

  aplicarFiltros() {
    // Si no hay datos, no hacemos nada
    if (!this.ventasOriginales) return;

    this.ventasFiltradas = this.ventasOriginales.filter(v => {
      // 1. Filtro por nombre (Verificamos que exista el objeto cliente)
      const nombreCliente = v.cliente?.nombre?.toLowerCase() || '';
      const coincideNombre = nombreCliente.includes(this.filtroNombre.toLowerCase());

      // 2. Filtro por fechas (Normalizamos a inicio del día para comparar)
      const fechaVenta = new Date(v.fecha).setHours(0,0,0,0);
      
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
  }

  limpiarFiltros() {
    this.filtroNombre = '';
    this.fechaInicio = '';
    this.fechaFin = '';
    this.ventasFiltradas = this.ventasOriginales;
  }

  // Agrégalo después de limpiarFiltros()
  calcularTotalReporte(): number {
    return this.ventasFiltradas.reduce((total, v) => {
      // Sumamos los subtotales de cada detalle de la venta
      const subtotalVenta = v.detalles?.reduce((sub: number, d: any) => 
        sub + (d.cantidad * d.precioVenta), 0) || 0;
      return total + subtotalVenta;
    }, 0);
  }
}