using ApiCineSharp.API.Data;
using ApiCineSharp.API.DTOs;
using ApiCineSharp.API.Servicios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiCineSharp.API.Servicios.Servicios
{
    public class PeliculaService : IPeliculaService
    {
        private readonly AppDbContext _context;
        public PeliculaService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<PeliculaDTO>> ObtenerPeliculasAsync()
        {
            var peliculas = await _context.Peliculas.ToListAsync();
            return peliculas.Select(p => new PeliculaDTO()
            {
                Id = p.Id,
                Clasificacion = p.Clasificacion,
                DuracionMinutos = p.DuracionMin,
                Imagen = p.PosterUrl,
                Titulo = p.Titulo
            }).ToList();
        }

        public async Task<DetallePeliculaDTO> ObtenerDetallePeliculaAsync(int peliculaId)
        {
            var pelicula = await _context.Peliculas
                .Include(p => p.Funciones)
                .ThenInclude(f => f.Sala)
                .FirstOrDefaultAsync(p => p.Id == peliculaId);

            if (pelicula == null)
            {
                return null;
            }

            return new DetallePeliculaDTO()
            {
                Id = peliculaId,
                Titulo = pelicula.Titulo,
                Clasificacion = pelicula.Clasificacion,
                Imagen = pelicula.PosterUrl,
                DuracionMinutos = pelicula.DuracionMin,
                Funciones = pelicula.Funciones.Select(f => new PeliculaFuncion()
                {
                    Fecha = f.Fecha,
                    Hora = f.HoraInicio,
                    Precio = f.Precio,
                    Sala = f.Sala.Nombre
                }).ToList(),
                Sinopsis = pelicula.Sinopsis

            };
        }

        public Task<List<PeliculaDTO>> BuscarPeliculasAsync(string titulo)
        {
            throw new NotImplementedException();
        }
    }
}
