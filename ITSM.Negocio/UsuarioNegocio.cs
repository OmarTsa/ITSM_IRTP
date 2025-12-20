using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Usuario?> LoginAsync(string usuario, string clave)
        {
            // CONSULTA CORREGIDA:
            // Compara contra las columnas reales de la BD.
            return await _contexto.Usuarios
                .FirstOrDefaultAsync(u => u.Username == usuario
                                       && u.PasswordHash == clave
                                       && u.Estado == 1);
        }

        public async Task<List<Usuario>> ListarUsuariosAsync()
        {
            return await _contexto.Usuarios.ToListAsync();
        }

        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int id)
        {
            return await _contexto.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> GuardarUsuarioAsync(Usuario usuario)
        {
            if (usuario.IdUsuario == 0)
            {
                _contexto.Usuarios.Add(usuario);
            }
            else
            {
                _contexto.Usuarios.Update(usuario);
            }

            await _contexto.SaveChangesAsync();
            return usuario;
        }
    }
}