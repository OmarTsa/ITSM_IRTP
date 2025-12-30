using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Entidades;
using BCrypt.Net;

namespace ITSM.Negocio
{
    public class UsuarioNegocio
    {
        private readonly ContextoBD _contexto;

        public UsuarioNegocio(ContextoBD contexto)
        {
            _contexto = contexto;
        }

        public async Task<Usuario?> LoginAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var usuario = await _contexto.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Username == username && u.Estado == 1);

            if (usuario == null) return null;

            bool esValido = false;
            try
            {
                if (!string.IsNullOrEmpty(usuario.PasswordHash))
                {
                    esValido = BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash);
                }
            }
            catch
            {
                esValido = false;
            }

            if (esValido)
            {
                return usuario;
            }

            return null;
        }

        public async Task CambiarContrasenaAsync(int idUsuario, string nuevaContrasena)
        {
            var usuario = await _contexto.Usuarios.FindAsync(idUsuario);
            if (usuario == null) throw new Exception("El usuario no existe.");

            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(nuevaContrasena);
            await _contexto.SaveChangesAsync();
        }

        public async Task<List<Usuario>> ListarUsuariosAsync()
        {
            return await _contexto.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Area)
                .AsNoTracking()
                .OrderBy(u => u.Apellidos)
                .ToListAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _contexto.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Area)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        public async Task RegistrarUsuarioAsync(Usuario usuario, string passwordPlano)
        {
            if (await ExisteUsernameAsync(usuario.Username))
                throw new Exception($"El nombre de usuario '{usuario.Username}' ya está en uso.");

            if (await ExisteDniAsync(usuario.Dni))
                throw new Exception($"El DNI '{usuario.Dni}' ya está registrado.");

            if (await ExisteCorreoAsync(usuario.Correo))
                throw new Exception($"El correo '{usuario.Correo}' ya está registrado.");

            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordPlano);
            usuario.Estado = 1;
            usuario.FechaBaja = null;
            usuario.FechaCreacion = DateTime.Now;
            usuario.Nombres = usuario.Nombres.ToUpper();
            usuario.Apellidos = usuario.Apellidos.ToUpper();
            usuario.Username = usuario.Username.ToLower().Trim();
            usuario.Correo = usuario.Correo.ToLower().Trim();

            _contexto.Usuarios.Add(usuario);
            await _contexto.SaveChangesAsync();
        }

        public async Task ActualizarUsuarioAsync(Usuario usuario)
        {
            var usuarioDb = await _contexto.Usuarios.FindAsync(usuario.IdUsuario);
            if (usuarioDb == null) throw new Exception("El usuario que intenta editar no existe.");

            if (await _contexto.Usuarios.AnyAsync(u => u.Username == usuario.Username && u.IdUsuario != usuario.IdUsuario))
                throw new Exception($"El usuario '{usuario.Username}' ya pertenece a otra persona.");

            if (await _contexto.Usuarios.AnyAsync(u => u.Dni == usuario.Dni && u.IdUsuario != usuario.IdUsuario))
                throw new Exception($"El DNI '{usuario.Dni}' ya pertenece a otra persona.");

            usuarioDb.Nombres = usuario.Nombres.ToUpper();
            usuarioDb.Apellidos = usuario.Apellidos.ToUpper();
            usuarioDb.Dni = usuario.Dni;
            usuarioDb.Correo = usuario.Correo.ToLower().Trim();
            usuarioDb.Username = usuario.Username.ToLower().Trim();
            usuarioDb.IdRol = usuario.IdRol;
            usuarioDb.IdArea = usuario.IdArea;
            usuarioDb.Cargo = usuario.Cargo;

            if (usuario.Estado != usuarioDb.Estado)
            {
                usuarioDb.Estado = usuario.Estado;
                if (usuario.Estado == 0)
                    usuarioDb.FechaBaja = DateTime.Now;
                else
                    usuarioDb.FechaBaja = null;
            }

            await _contexto.SaveChangesAsync();
        }

        public async Task DarDeBajaAsync(int idUsuario)
        {
            var usuario = await _contexto.Usuarios.FindAsync(idUsuario);
            if (usuario == null) throw new Exception("Usuario no encontrado.");

            usuario.Estado = 0;
            usuario.FechaBaja = DateTime.Now;
            await _contexto.SaveChangesAsync();
        }

        public async Task<bool> ExisteUsernameAsync(string username)
        {
            return await _contexto.Usuarios.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> ExisteDniAsync(string dni)
        {
            return await _contexto.Usuarios.AnyAsync(u => u.Dni == dni);
        }

        public async Task<bool> ExisteCorreoAsync(string correo)
        {
            return await _contexto.Usuarios.AnyAsync(u => u.Correo == correo);
        }

        public async Task<List<Rol>> ListarRolesActivosAsync()
        {
            return await _contexto.Roles.Where(r => r.Estado == 1).ToListAsync();
        }

        public async Task<List<Area>> ListarAreasAsync()
        {
            return await _contexto.Areas.OrderBy(a => a.Nombre).ToListAsync();
        }

        public async Task<Usuario?> ObtenerPorUsernameAsync(string username)
        {
            return await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
