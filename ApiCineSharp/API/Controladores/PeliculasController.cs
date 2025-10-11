using ApiCineSharp.API.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiCineSharp.API.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaService _servicioPeliculas;

        public PeliculasController(IPeliculaService servicioPeliculas)
        {
            _servicioPeliculas = servicioPeliculas;
        }

        [HttpGet("obtener-todas")]
        public async Task<IActionResult> GetPeliculas()
        {
            var peliculas = await _servicioPeliculas.ObtenerPeliculasAsync();
            return Ok(peliculas);
        }

        [HttpGet("detalle/{id}")]
        public async Task<IActionResult> GetDetallePelicula(int id)
        {
            var pelicula = await _servicioPeliculas.ObtenerDetallePeliculaAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }
            return Ok(pelicula);
        }
    }
}
