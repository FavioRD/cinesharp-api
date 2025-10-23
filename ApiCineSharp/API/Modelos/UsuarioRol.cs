namespace ApiCineSharp.API.Modelos
{
    public class UsuarioRol
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int RolId { get; set; }
        public DateTime FechaAsignacion { get; set; } = DateTime.Now;

        // Relaciones
        public Usuario? Usuario { get; set; }
        public Rol? Rol { get; set; }
    }
}
