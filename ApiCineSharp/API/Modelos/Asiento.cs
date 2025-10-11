namespace ApiCineSharp.API.Modelos
{
    public class Asiento
    {
        public int Id { get; set; }
        public int SalaId { get; set; }
        public char Fila { get; set; }
        public int Numero { get; set; }
        public string Estado { get; set; } = "Activo";

        // Relaciones
        public Sala? Sala { get; set; }
        public ICollection<Entrada>? Entradas { get; set; }
    }
}
