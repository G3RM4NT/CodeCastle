using System.Text.Json.Serialization;

namespace WorkTestAPI.Models
{
    public class CompraDetalle
    {
        public int Id { get; set; }
        public int CompraId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioCompra { get; set; }

        public Producto Producto { get; set; }
       
        [JsonIgnore]
        public Compra Compra { get; set; }

        
    }
}