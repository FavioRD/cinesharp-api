namespace ApiCineSharp.API.Modelos
{
    public class Sala
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int Capacidad { get; set; }

        // Relaciones
        public ICollection<Asiento>? Asientos { get; set; }
        public ICollection<Funcion>? Funciones { get; set; }
    }
}
