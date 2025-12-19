// 1. REFERENCIAS DE SISTEMA (¡Esto es lo que falta para arreglar List y Task!)
using System.Collections.Generic;
using System.Threading.Tasks;

// 2. REFERENCIAS DE ENTITY FRAMEWORK
using Microsoft.EntityFrameworkCore;

// 3. REFERENCIAS DE TU PROYECTO
using ITSM.Entidades;
using ITSM.Datos;

namespace ITSM.Negocio
{
    public class UsuarioNegocio
    {
        private readonly ContextoBD _contexto;

        public UsuarioNegocio(ContextoBD contexto)
        {
            _contexto = contexto;
        }

        // Login simple (para Tesis)
        public async Task<Usuario?> LoginAsync(string usuario, string clave)
        {
            return await _contexto.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == usuario && u.Clave == clave && u.Estado == 1);
        }

        // Listar todos los usuarios
        public async Task<List<Usuario>> ListarUsuariosAsync()
        {
            return await _contexto.Usuarios.ToListAsync();
        }

        // Obtener uno por ID
        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int id)
        {
            return await _contexto.Usuarios.FindAsync(id);
        }

        // Guardar (Crear o Editar)
        public async Task<Usuario> GuardarUsuarioAsync(Usuario usuario)
        {
            if (usuario.IdUsuario == 0)
            {
                // Nuevo
                _contexto.Usuarios.Add(usuario);
            }
            else
            {
                // Editar
                _contexto.Usuarios.Update(usuario);
            }

            await _contexto.SaveChangesAsync();
            return usuario;
        }
    }
}