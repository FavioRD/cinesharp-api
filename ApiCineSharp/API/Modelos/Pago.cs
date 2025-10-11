namespace ApiCineSharp.API.Modelos
{
    public class Pago
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int EntradaId { get; set; }
        public string Metodo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public string? CodigoTransaccion { get; set; }

        // Relaciones
        public Usuario? Usuario { get; set; }
        public Entrada? Entrada { get; set; }
    }
}
