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

        // =================================================================================
        // 1. AUTENTICACIÓN Y SEGURIDAD
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

            // Verificamos contraseña hash
            // Nota: BCrypt maneja internamente el salt, por eso solo pasamos el password plano y el hash
            bool esValido = BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash);

            if (esValido)
            {
                // Aquí podrías registrar un log de acceso exitoso en la tabla SEG_INTENTOS_ACCESO
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

        // =================================================================================
        // 2. GESTIÓN DE USUARIOS (CRUD)
        // =================================================================================

        public async Task<List<Usuario>> ListarUsuariosAsync()
        {
            // Traemos la lista completa con sus relaciones (Rol y Área) para mostrar en la grilla
            // Usamos AsNoTracking() para mejorar el rendimiento ya que es solo lectura
            return await _contexto.Usuarios
                .Include(u => u.Rol)
                //.Include(u => u.Area) // Descomentar cuando hayas agregado la propiedad virtual Area en Usuario.cs
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

            // 2. Preparar datos por defecto
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordPlano);
            usuario.Estado = 1; // Activo
            usuario.FechaBaja = null;

            // Convertir a mayúsculas para estandarizar
            usuario.Nombres = usuario.Nombres.ToUpper();
            usuario.Apellidos = usuario.Apellidos.ToUpper();
            usuario.Username = usuario.Username.ToLower(); // Usernames en minúscula
            usuario.Correo = usuario.Correo.ToLower();

            // 3. Guardar
            _contexto.Usuarios.Add(usuario);
            await _contexto.SaveChangesAsync();
        }

        public async Task ActualizarUsuarioAsync(Usuario usuario)
        {
            var usuarioDb = await _contexto.Usuarios.FindAsync(usuario.IdUsuario);
            if (usuarioDb == null) throw new Exception("El usuario que intenta editar no existe.");

            // 1. Validaciones de duplicados (Excluyendo al propio usuario)
            if (await _contexto.Usuarios.AnyAsync(u => u.Username == usuario.Username && u.IdUsuario != usuario.IdUsuario))
                throw new Exception($"El usuario '{usuario.Username}' ya pertenece a otra persona.");

            if (await _contexto.Usuarios.AnyAsync(u => u.Dni == usuario.Dni && u.IdUsuario != usuario.IdUsuario))
                throw new Exception($"El DNI '{usuario.Dni}' ya pertenece a otra persona.");

            // 2. Actualizar campos permitidos
            usuarioDb.Nombres = usuario.Nombres.ToUpper();
            usuarioDb.Apellidos = usuario.Apellidos.ToUpper();
            usuarioDb.Dni = usuario.Dni;
            usuarioDb.Correo = usuario.Correo.ToLower();
            usuarioDb.Username = usuario.Username.ToLower();
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

            // Nota: NO actualizamos el PasswordHash aquí. Eso se hace en CambiarContrasenaAsync.

            await _contexto.SaveChangesAsync();
        }

        public async Task DarDeBajaAsync(int idUsuario)
        {
            var usuario = await _contexto.Usuarios.FindAsync(idUsuario);
            if (usuario == null) throw new Exception("Usuario no encontrado.");

            // Validación: No permitir dar de baja al propio admin si es el único (regla de seguridad opcional)

            usuario.Estado = 0; // Inactivo
            usuario.FechaBaja = DateTime.Now;

            await _contexto.SaveChangesAsync();
        }

        // =================================================================================
        // 3. MÉTODOS AUXILIARES (Validaciones y Combos)
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

        // Para llenar los ComboBox (Selects) en la interfaz
        public async Task<List<Rol>> ListarRolesActivosAsync()
        {
            return await _contexto.Roles
                .Where(r => r.Estado == 1)
                .ToListAsync();
        }

        public async Task<List<Area>> ListarAreasAsync()
        {
            return await _contexto.Areas
                .OrderBy(a => a.Nombre)
                .ToListAsync();
        }
    }
}