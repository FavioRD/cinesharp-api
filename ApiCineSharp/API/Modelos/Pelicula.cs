namespace ApiCineSharp.API.Modelos
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Sinopsis { get; set; }
        public int DuracionMin { get; set; }
        public string? Clasificacion { get; set; }
        public string? Idioma { get; set; }
        public bool Subtitulos { get; set; }
        public string? PosterUrl { get; set; }
        public string Estado { get; set; } = "Activa";

        // Relaciones
        public ICollection<Funcion>? Funciones { get; set; }
    }
}
