import { Component, OnInit, ChangeDetectorRef } from '@angular/core'; // 1. Importar ChangeDetectorRef
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CompraService } from '../../services/compra.service';

@Component({
  selector: 'app-reporte-compras',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl:'./reportecompras.html'
})
export class ReporteComprasComponent implements OnInit {
  comprasOriginales: any[] = [];
  comprasFiltradas: any[] = [];

  filtroProveedor: string = '';
  fechaInicio: string = '';
  fechaFin: string = '';

  // 2. Inyectar en el constructor
  constructor(
    private compraService: CompraService,
    private cdr: ChangeDetectorRef 
  ) {}

  ngOnInit() {
    this.cargarReporte();
  }

  cargarReporte() {
    this.compraService.getAll().subscribe({
      next: (res: any[]) => {
        // 3. Usar spread para nueva referencia y forzar detección
        this.comprasOriginales = [...res];
        this.comprasFiltradas = [...res];
        this.cdr.detectChanges(); 
      },
      error: (err) => console.error("ERROR DE API:", err)
    });
  }

  aplicarFiltros() {
    if (!this.comprasOriginales) return;
    this.comprasFiltradas = this.comprasOriginales.filter(c => {
      const proveedorObj = c.Proveedor || c.proveedor;
      const nombreProv = proveedorObj?.Nombre?.toLowerCase() || proveedorObj?.nombre?.toLowerCase() || '';
      const coincideNombre = nombreProv.includes(this.filtroProveedor.toLowerCase());

      const fechaRaw = c.Fecha || c.fecha;
      const fechaCompra = new Date(fechaRaw).setHours(0,0,0,0);
      
      let coincideFecha = true;
      if (this.fechaInicio) {
        const inicio = new Date(this.fechaInicio).setHours(0,0,0,0);
        if (fechaCompra < inicio) coincideFecha = false;
      }
      if (this.fechaFin) {
        const fin = new Date(this.fechaFin).setHours(23,59,59,999);
        if (fechaCompra > fin) coincideFecha = false;
      }
      return coincideNombre && coincideFecha;
    });
    this.cdr.detectChanges(); // También al filtrar
  }

  limpiarFiltros() {
    this.filtroProveedor = '';
    this.fechaInicio = '';
    this.fechaFin = '';
    this.comprasFiltradas = [...this.comprasOriginales];
    this.cdr.detectChanges();
  }

  calcularTotalInversion(): number {
    return this.comprasFiltradas.reduce((total, c) => {
      const monto = c.Total ?? c.total ?? 0;
      return total + monto;
    }, 0);
  }
}