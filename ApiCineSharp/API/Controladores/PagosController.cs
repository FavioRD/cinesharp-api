using ApiCineSharp.API.Data;
using ApiCineSharp.API.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCineSharp.API.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PagosController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Pagos/procesar-pago
        [HttpPost("procesar-pago")]
        public async Task<IActionResult> ProcesarPago([FromBody] PagoRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Verificar que los asientos estén disponibles
                var asientosOcupados = await _context.Entradas
                    .Where(e => e.FuncionId == request.FuncionId &&
                           request.AsientosIds.Contains(e.AsientoId) &&
                           e.Estado != "Cancelada")
                    .Select(e => e.AsientoId)
                    .ToListAsync();

                if (asientosOcupados.Any())
                {
                    return BadRequest($"Los asientos {string.Join(", ", asientosOcupados)} ya están ocupados");
                }

                // 2. Crear o obtener usuario (simplificado - en un caso real usarías autenticación)
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == request.UsuarioInfo.Email);

                if (usuario == null)
                {
                    // Crear usuario temporal (en un caso real esto sería diferente)
                    usuario = new Usuario
                    {
                        NombreCompleto = request.UsuarioInfo.Nombre,
                        Email = request.UsuarioInfo.Email,
                        Telefono = request.UsuarioInfo.Telefono,
                        ContrasenaHash = "temp_hash", // En realidad esto vendría del registro
                        FechaRegistro = DateTime.Now
                    };
                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();
                }

                var entradasCreadas = new List<Entrada>();
                var codigoTransaccion = $"TXN{Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper()}";

                // 3. Crear entradas para cada asiento
                foreach (var asientoId in request.AsientosIds)
                {
                    var entrada = new Entrada
                    {
                        FuncionId = request.FuncionId,
                        AsientoId = asientoId,
                        UsuarioId = usuario.Id,
                        Estado = "Pagada",
                        FechaReserva = DateTime.Now
                    };

                    _context.Entradas.Add(entrada);
                    entradasCreadas.Add(entrada);
                }

                await _context.SaveChangesAsync();

                // 4. Crear pago
                var pago = new Pago
                {
                    UsuarioId = usuario.Id,
                    EntradaId = entradasCreadas.First().Id, // Relación 1:1 simplificada
                    Metodo = request.MetodoPago,
                    Monto = request.Total,
                    FechaPago = DateTime.Now,
                    CodigoTransaccion = codigoTransaccion
                };

                _context.Pagos.Add(pago);
                await _context.SaveChangesAsync();

                // 5. Confirmar transacción
                await transaction.CommitAsync();

                // 6. Retornar respuesta
                return Ok(new
                {
                    Success = true,
                    CodigoTransaccion = codigoTransaccion,
                    Entradas = entradasCreadas.Select(e => new { e.Id, AsientoId = e.AsientoId }),
                    Total = request.Total,
                    Mensaje = "Pago procesado exitosamente"
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error al procesar el pago: {ex.Message}");
            }
        }
    }

    // Clase para el request del pago
    public class PagoRequest
    {
        public int FuncionId { get; set; }
        public List<int> AsientosIds { get; set; } = new();
        public decimal Total { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public UsuarioInfo UsuarioInfo { get; set; } = new();
    }

    public class UsuarioInfo
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }
}