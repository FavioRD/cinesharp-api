using ApiCineSharp.API.Data;
using ApiCineSharp.API.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCineSharp.API.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FuncionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Funciones/pelicula/{peliculaId}
        [HttpGet("pelicula/{peliculaId}")]
        public async Task<IActionResult> GetFuncionesPorPelicula(int peliculaId)
        {
            try
            {
                var funciones = await _context.Funciones
                    .Include(f => f.Sala)
                    .Include(f => f.Pelicula)
                    .Where(f => f.PeliculaId == peliculaId && f.Fecha >= DateTime.Today)
                    .OrderBy(f => f.Fecha)
                    .ThenBy(f => f.HoraInicio)
                    .Select(f => new
                    {
                        f.Id,
                        f.Fecha,
                        f.HoraInicio,
                        f.HoraFin,
                        f.Precio,
                        Sala = f.Sala.Nombre,
                        TipoSala = f.Sala.Tipo,
                        Pelicula = f.Pelicula.Titulo
                    })
                    .ToListAsync();

                return Ok(funciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Funciones/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFuncion(int id)
        {
            try
            {
                var funcion = await _context.Funciones
                    .Include(f => f.Sala)
                    .Include(f => f.Pelicula)
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (funcion == null)
                    return NotFound();

                var funcionDto = new
                {
                    funcion.Id,
                    funcion.Fecha,
                    funcion.HoraInicio,
                    funcion.HoraFin,
                    funcion.Precio,
                    Sala = funcion.Sala.Nombre,
                    TipoSala = funcion.Sala.Tipo,
                    Pelicula = funcion.Pelicula.Titulo
                };

                return Ok(funcionDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}