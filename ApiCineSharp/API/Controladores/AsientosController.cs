using ApiCineSharp.API.Data;
using ApiCineSharp.API.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCineSharp.API.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsientosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AsientosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Asientos/funcion/{funcionId}
        [HttpGet("funcion/{funcionId}")]
        public async Task<IActionResult> GetAsientosPorFuncion(int funcionId)
        {
            try
            {
                // Obtener la función
                var funcion = await _context.Funciones
                    .Include(f => f.Sala)
                    .FirstOrDefaultAsync(f => f.Id == funcionId);

                if (funcion == null)
                    return NotFound("Función no encontrada");

                // Obtener todos los asientos de la sala
                var asientosSala = await _context.Asientos
                    .Where(a => a.SalaId == funcion.SalaId && a.Estado == "Activo")
                    .OrderBy(a => a.Fila)
                    .ThenBy(a => a.Numero)
                    .ToListAsync();

                // Obtener asientos ya ocupados en esta función
                var asientosOcupados = await _context.Entradas
                    .Where(e => e.FuncionId == funcionId && e.Estado != "Cancelada")
                    .Select(e => e.AsientoId)
                    .ToListAsync();

                // Estado de disponibilidad
                var asientosConEstado = asientosSala.Select(a => new
                {
                    a.Id,
                    a.Fila,
                    a.Numero,
                    a.SalaId,
                    Disponible = !asientosOcupados.Contains(a.Id),
                    Codigo = $"{a.Fila}{a.Numero}"
                }).ToList();

                return Ok(new
                {
                    Sala = funcion.Sala.Nombre,
                    TipoSala = funcion.Sala.Tipo,
                    Capacidad = funcion.Sala.Capacidad,
                    Asientos = asientosConEstado,
                    Precio = funcion.Precio
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}