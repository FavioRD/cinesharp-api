namespace ApiCineSharp.API.Modelos
{
    public class Entrada
    {
        public int Id { get; set; }
        public int FuncionId { get; set; }
        public int AsientoId { get; set; }
        public int UsuarioId { get; set; }
        public string Estado { get; set; } = "Reservada";
        public DateTime FechaReserva { get; set; }

        // Relaciones
        public Funcion? Funcion { get; set; }
        public Asiento? Asiento { get; set; }
        public Usuario? Usuario { get; set; }
        public ICollection<Pago>? Pagos { get; set; }
    }
}
