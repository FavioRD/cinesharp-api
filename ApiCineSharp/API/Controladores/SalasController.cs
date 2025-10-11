using ApiCineSharp.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCineSharp.API.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SalasController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> ObtenerSalas()
        {
            var salas = await _context.Salas.ToListAsync();
            return Ok(salas);
        }
    }
}
