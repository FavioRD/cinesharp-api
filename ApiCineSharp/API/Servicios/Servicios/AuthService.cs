using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiCineSharp.API.Data;
using ApiCineSharp.API.DTOs;
using ApiCineSharp.API.Modelos;
using ApiCineSharp.API.Servicios.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ApiCineSharp.API.Servicios.Servicios
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _cfg;

        public AuthService(AppDbContext db, IConfiguration cfg)
        {
            _db = db;
            _cfg = cfg;
        }

        // ==============================================================
        // REGISTRO
        // ==============================================================
        public async Task<RespuestaAuthDTO> Registrar(RegistrarCredencialesDTO credenciales)
        {
            // Validar si ya existe
            var existe = await _db.Usuarios.AnyAsync(u => u.Email == credenciales.Email);
            if (existe)
                throw new Exception("El correo ya está registrado.");

            // Hashear la contraseña
            var hash = BCrypt.Net.BCrypt.HashPassword(credenciales.Contrasena);

            // Crear usuario
            var nuevo = new Usuario
            {
                NombreCompleto = $"{credenciales.Nombre} {credenciales.Apellido}".Trim(),
                Email = credenciales.Email,
                ContrasenaHash = hash,
                FechaRegistro = DateTime.Now,
                Telefono = credenciales.Telefono
            };

            _db.Usuarios.Add(nuevo);
            await _db.SaveChangesAsync();

            // Asignar rol "Cliente" por defecto
            var rolCliente = await _db.Roles.FirstOrDefaultAsync(r => r.Nombre == "Cliente");
            if (rolCliente != null)
            {
                _db.UsuarioRoles.Add(new UsuarioRol
                {
                    UsuarioId = nuevo.Id,
                    RolId = rolCliente.Id
                });
                await _db.SaveChangesAsync();
            }

            // Generar token
            var roles = new List<string> { "Cliente" };
            var token = CrearToken(nuevo, roles);
            return new RespuestaAuthDTO
            {
                Mensaje = "Cuenta creada exitosamente.",
                Token = token
            };
        }

        // ==============================================================
        // LOGIN
        // ==============================================================
        public async Task<RespuestaAuthDTO> IniciarSesion(CredencialesLoginDTO credenciales)
        {
            var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == credenciales.Email);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            if (!BCrypt.Net.BCrypt.Verify(credenciales.Contrasena, usuario.ContrasenaHash))
                throw new Exception("Contraseña incorrecta.");

            // Buscar roles
            var roles = await _db.UsuarioRoles
                .Where(ur => ur.UsuarioId == usuario.Id)
                .Include(ur => ur.Rol)
                .Select(ur => ur.Rol!.Nombre)
                .ToListAsync();

            // Generar token
            var token = CrearToken(usuario, roles);
            return new RespuestaAuthDTO()
            {
                Mensaje = "Inicio de sesion exitoso",
                Token = token,
            };
        }

        // ==============================================================
        // TOKEN JWT
        // ==============================================================
        private string CrearToken(Usuario usuario, IEnumerable<string> roles)
        {
            var jwt = _cfg.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new(ClaimTypes.Email, usuario.Email),
                new("nombreCompleto", usuario.NombreCompleto)
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpiresMinutes"]!)),
                signingCredentials: creds
            );

            var res = new JwtSecurityTokenHandler().WriteToken(token);

            return res;
        }
    }
}

