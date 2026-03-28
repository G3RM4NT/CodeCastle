namespace WorkTestAPI.Models
{
    public class VentaDetalle
    {
        public int Id { get; set; }
        public int VentaId { get; set; } // La llave física

        // ESTA ES LA QUE NECESITA EL DBCONTEXT:
        public Venta Venta { get; set; }

        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioVenta { get; set; }
        public Producto Producto { get; set; }
    }
}