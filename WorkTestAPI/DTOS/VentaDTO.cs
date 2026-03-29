using System.Text.Json.Serialization;

namespace WorkTestAPI.DTOS
{
public class VentaDTO
{
    public int ClienteId { get; set; }
    public int UsuarioId { get; set; } // <--- Agregamos este campo
    public List<VentaDetalleDTO> Detalles { get; set; } = new();
}

    public class VentaDetalleDTO
    {
        [JsonPropertyName("productoId")]
        public int ProductoId { get; set; }

        [JsonPropertyName("cantidad")]
        public int Cantidad { get; set; }

        [JsonPropertyName("precioVenta")]
        public decimal PrecioVenta { get; set; }
    }
}