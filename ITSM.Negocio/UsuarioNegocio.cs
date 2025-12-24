using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Entidades;
// Asegúrate de haber instalado el paquete NuGet: BCrypt.Net-Next
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

        // =================================================================================
        // 1. AUTENTICACIÓN Y SEGURIDAD (CON BCRYPT)
        // =================================================================================

        public async Task<Usuario?> LoginAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            // Buscamos usuario activo (Estado = 1) e incluimos su Rol para validar permisos
            var usuario = await _contexto.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Username == username && u.Estado == 1);

            // Si no existe o está inactivo
            if (usuario == null) return null;

            // Verificamos contraseña usando BCrypt
            // Verify compara el texto plano con el hash almacenado
            bool esValido = false;
            try
            {
                // Manejo de errores por si hay hashes antiguos no compatibles o nulos
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

            // Hasheamos la nueva contraseña antes de guardar
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(nuevaContrasena);
            await _contexto.SaveChangesAsync();
        }

        // =================================================================================
        // 2. GESTIÓN DE USUARIOS (CRUD COMPLETO)
        // =================================================================================

        public async Task<List<Usuario>> ListarUsuariosAsync()
        {
            return await _contexto.Usuarios
                .Include(u => u.Rol)
                .AsNoTracking()
                .OrderBy(u => u.Apellidos)
                .ToListAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _contexto.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        public async Task RegistrarUsuarioAsync(Usuario usuario, string passwordPlano)
        {
            // 1. Validaciones de Negocio
            if (await ExisteUsernameAsync(usuario.Username))
                throw new Exception($"El nombre de usuario '{usuario.Username}' ya está en uso.");

            if (await ExisteDniAsync(usuario.Dni))
                throw new Exception($"El DNI '{usuario.Dni}' ya está registrado.");

            if (await ExisteCorreoAsync(usuario.Correo))
                throw new Exception($"El correo '{usuario.Correo}' ya está registrado.");

            // 2. Seguridad: Hashear contraseña
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordPlano);

            // 3. Datos por defecto
            usuario.Estado = 1; // Activo
            usuario.FechaBaja = null;
            usuario.FechaCreacion = DateTime.Now;

            // Estandarización de texto
            usuario.Nombres = usuario.Nombres.ToUpper();
            usuario.Apellidos = usuario.Apellidos.ToUpper();
            usuario.Username = usuario.Username.ToLower().Trim();
            usuario.Correo = usuario.Correo.ToLower().Trim();

            // 4. Guardar
            _contexto.Usuarios.Add(usuario);
            await _contexto.SaveChangesAsync();
        }

        public async Task ActualizarUsuarioAsync(Usuario usuario)
        {
            var usuarioDb = await _contexto.Usuarios.FindAsync(usuario.IdUsuario);
            if (usuarioDb == null) throw new Exception("El usuario que intenta editar no existe.");

            // Validaciones de duplicados (Excluyendo al propio usuario)
            if (await _contexto.Usuarios.AnyAsync(u => u.Username == usuario.Username && u.IdUsuario != usuario.IdUsuario))
                throw new Exception($"El usuario '{usuario.Username}' ya pertenece a otra persona.");

            if (await _contexto.Usuarios.AnyAsync(u => u.Dni == usuario.Dni && u.IdUsuario != usuario.IdUsuario))
                throw new Exception($"El DNI '{usuario.Dni}' ya pertenece a otra persona.");

            // Actualizar campos permitidos (NO tocamos el PasswordHash aquí)
            usuarioDb.Nombres = usuario.Nombres.ToUpper();
            usuarioDb.Apellidos = usuario.Apellidos.ToUpper();
            usuarioDb.Dni = usuario.Dni;
            usuarioDb.Correo = usuario.Correo.ToLower().Trim();
            usuarioDb.Username = usuario.Username.ToLower().Trim();
            usuarioDb.IdRol = usuario.IdRol;
            usuarioDb.IdArea = usuario.IdArea;
            usuarioDb.Cargo = usuario.Cargo;

            // Manejo de Estado (Baja/Reactivación)
            if (usuario.Estado != usuarioDb.Estado)
            {
                usuarioDb.Estado = usuario.Estado;
                if (usuario.Estado == 0) // Si se está desactivando
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

            usuario.Estado = 0; // Inactivo
            usuario.FechaBaja = DateTime.Now;

            await _contexto.SaveChangesAsync();
        }

        // =================================================================================
        // 3. MÉTODOS AUXILIARES
        // =================================================================================

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
    }
}