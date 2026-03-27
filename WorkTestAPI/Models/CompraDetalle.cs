namespace WorkTestAPI.Models
{
    public class CompraDetalle
    {
        public int Id { get; set; }
        public int CompraId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioCompra { get; set; }
    }
}