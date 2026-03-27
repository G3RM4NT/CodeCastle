namespace WorkTestAPI.Models
{
    public class Compra
    {
        public int Id { get; set; }
        public int ProveedorId { get; set; }
        public DateTime Fecha { get; set; }
        public int? UsuarioId { get; set; }

        public List<CompraDetalle> Detalles { get; set; } = new();
    }
}