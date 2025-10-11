namespace ApiCineSharp.API.Modelos
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContrasenaHash { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public DateTime FechaRegistro { get; set; }

        // Relaciones
        public ICollection<Entrada>? Entradas { get; set; }
        public ICollection<Pago>? Pagos { get; set; }
    }
}
