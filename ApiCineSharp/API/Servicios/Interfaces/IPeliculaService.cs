using ApiCineSharp.API.DTOs;

namespace ApiCineSharp.API.Servicios.Interfaces
{
    public interface IPeliculaService
    {
        public Task<List<PeliculaDTO>> ObtenerPeliculasAsync();
        public Task<DetallePeliculaDTO> ObtenerDetallePeliculaAsync(int peliculaId);
        public Task<List<PeliculaDTO>> BuscarPeliculasAsync(string titulo);
    }
}
