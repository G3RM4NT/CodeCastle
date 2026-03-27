namespace WorkTestAPI.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
    }
}