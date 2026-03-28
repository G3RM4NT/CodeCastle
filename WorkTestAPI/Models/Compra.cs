namespace WorkTestAPI.Models
{
    public class Compra
    {
        public int Id { get; set; }
        public int ProveedorId { get; set; }
        public DateTime Fecha { get; set; }
        public int? UsuarioId { get; set; }

        // Navegación
        public Proveedor? Proveedor { get; set; }

        // Mantenemos SOLO este nombre para los hijos
        public List<CompraDetalle> CompraDetalles { get; set; } = new List<CompraDetalle>();
    }
}