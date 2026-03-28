namespace WorkTestAPI.DTOS
{
    public class VentaDTO
    {
        public int ClienteId { get; set; }
        public List<VentaDetalleDTO> Detalles { get; set; } = new();
    }

    public class VentaDetalleDTO
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioVenta { get; set; }
    }
}

