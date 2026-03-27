namespace WorkTestAPI.DTOS
{
    public class CompraDTO
    {
        public int ProveedorId { get; set; }
        public List<CompraDetalleDTO> Detalles { get; set; } = new();
    }

    public class CompraDetalleDTO
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioCompra { get; set; }
    }
}