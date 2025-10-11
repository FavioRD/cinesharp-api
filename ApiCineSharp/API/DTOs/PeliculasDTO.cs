namespace ApiCineSharp.API.DTOs
{
    public class PeliculaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Imagen { get; set; }
        public string Clasificacion { get; set; }
        public int DuracionMinutos { get; set; }
    }

    public class DetallePeliculaDTO : PeliculaDTO
    {
        public string Sinopsis { get; set; }
        public List<PeliculaFuncion> Funciones { get; set; }
    }

    public class PeliculaFuncion
    {
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public decimal Precio { get; set; }
        public string Sala { get; set; }
    }
}
