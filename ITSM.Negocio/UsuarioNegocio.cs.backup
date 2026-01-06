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
            var usuarioExistente = await _contexto.Usuarios
                .Where(u => u.Username == usuario.Username)
                .ToListAsync();

            if (usuarioExistente.Any())
                throw new Exception($"El nombre de usuario '{usuario.Username}' ya está en uso.");

            var dniExistente = await _contexto.Usuarios
                .Where(u => u.Dni == usuario.Dni)
                .ToListAsync();

            if (dniExistente.Any())
                throw new Exception($"El DNI '{usuario.Dni}' ya está registrado.");

            var correoExistente = await _contexto.Usuarios
                .Where(u => u.Correo == usuario.Correo)
                .ToListAsync();

            if (correoExistente.Any())
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

            var usuariosConMismoUsername = await _contexto.Usuarios
                .Where(u => u.Username == usuario.Username && u.IdUsuario != usuario.IdUsuario)
                .ToListAsync();

            if (usuariosConMismoUsername.Any())
                throw new Exception($"El usuario '{usuario.Username}' ya pertenece a otra persona.");

            var usuariosConMismoDni = await _contexto.Usuarios
                .Where(u => u.Dni == usuario.Dni && u.IdUsuario != usuario.IdUsuario)
                .ToListAsync();

            if (usuariosConMismoDni.Any())
                throw new Exception($"El DNI '{usuario.Dni}' ya pertenece a otra persona.");

            Console.WriteLine($"📝 [BACKEND] ActualizarUsuarioAsync - Usuario: {usuarioDb.Username}");
            Console.WriteLine($"📝 [BACKEND] Estado ANTES: {usuarioDb.Estado} | Estado NUEVO: {usuario.Estado}");
            Console.WriteLine($"📝 [BACKEND] FechaBaja ANTES: {usuarioDb.FechaBaja} | FechaBaja NUEVA: {usuario.FechaBaja}");

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
                {
                    usuarioDb.FechaBaja = DateTime.Now;
                    Console.WriteLine($"🔴 [BACKEND] Usuario '{usuarioDb.Username}' DESACTIVADO - FechaBaja: {usuarioDb.FechaBaja}");
                }
                else
                {
                    usuarioDb.FechaBaja = null;
                    Console.WriteLine($"🟢 [BACKEND] Usuario '{usuarioDb.Username}' REACTIVADO - FechaBaja: NULL");
                }
            }

            Console.WriteLine($"💾 [BACKEND] Guardando cambios en la base de datos...");
            await _contexto.SaveChangesAsync();
            Console.WriteLine($"✅ [BACKEND] Usuario actualizado exitosamente");
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
            var usuarios = await _contexto.Usuarios
                .Where(u => u.Username == username)
                .ToListAsync();
            return usuarios.Any();
        }

        public async Task<bool> ExisteDniAsync(string dni)
        {
            var usuarios = await _contexto.Usuarios
                .Where(u => u.Dni == dni)
                .ToListAsync();
            return usuarios.Any();
        }

        public async Task<bool> ExisteCorreoAsync(string correo)
        {
            var usuarios = await _contexto.Usuarios
                .Where(u => u.Correo == correo)
                .ToListAsync();
            return usuarios.Any();
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
