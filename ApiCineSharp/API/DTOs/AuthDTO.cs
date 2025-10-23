namespace ApiCineSharp.API.DTOs
{
    public class AuthDTO
    {
    }

    public class CredencialesLoginDTO
    {
        public string Email { get; set; }
        public string Contrasena { get; set; }
    }
    public class RegistrarCredencialesDTO
    {
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
    }

    public class RespuestaAuthDTO
    {
        public string? Token { get; set; }
        public string Mensaje { get; set; }
    }

}
