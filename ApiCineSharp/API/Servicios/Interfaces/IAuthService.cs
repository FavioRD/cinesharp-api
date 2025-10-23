using ApiCineSharp.API.DTOs;

namespace ApiCineSharp.API.Servicios.Interfaces
{
    public interface IAuthService
    {
        public Task<RespuestaAuthDTO> IniciarSesion(CredencialesLoginDTO credenciales);
        public Task<RespuestaAuthDTO> Registrar(RegistrarCredencialesDTO credenciales);
    }
}
