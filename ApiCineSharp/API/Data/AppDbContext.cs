using ApiCineSharp.API.Modelos;
using Microsoft.EntityFrameworkCore;

namespace ApiCineSharp.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Sala> Salas { get; set; }
        public DbSet<Asiento> Asientos { get; set; }
        public DbSet<Funcion> Funciones { get; set; }
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UsuarioRol> UsuarioRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ejemplo de configuración simple:
            modelBuilder.Entity<Entrada>()
                .HasIndex(e => new { e.FuncionId, e.AsientoId })
                .IsUnique();
        }
    }
}
