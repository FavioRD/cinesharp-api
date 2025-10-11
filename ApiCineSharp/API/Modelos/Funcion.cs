namespace ApiCineSharp.API.Modelos
{
    public class Funcion
    {
        public int Id { get; set; }
        public int PeliculaId { get; set; }
        public int SalaId { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public decimal Precio { get; set; }

        // Relaciones
        public Pelicula? Pelicula { get; set; }
        public Sala? Sala { get; set; }
        public ICollection<Entrada>? Entradas { get; set; }
    }
}
