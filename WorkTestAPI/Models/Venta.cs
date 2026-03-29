namespace WorkTestAPI.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime Fecha { get; set; }
        public int? UsuarioId { get; set; }

        public virtual Cliente? Cliente { get; set; }
        public List<VentaDetalle> Detalles { get; set; } = new();

    }
}