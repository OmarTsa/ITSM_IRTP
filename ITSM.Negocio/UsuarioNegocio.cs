using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ITSM.Datos;
using ITSM.Entidades;

namespace ITSM.Negocio
{
    public class UsuarioNegocio
    {
        private readonly ContextoBD _contexto;

        public UsuarioNegocio(ContextoBD contexto)
        {
            _contexto = contexto;
        }

        // Método para validar el login (ya existía)
        public async Task<Usuario?> ValidarAccesoReal(string username, string password)
        {
            return await _contexto.Usuarios
                .Where(u => u.NombreUsuario == username && u.Clave == password && u.Estado == 1)
                .FirstOrDefaultAsync();
        }

        // --- NUEVO MÉTODO: Listar todos los usuarios ---
        public async Task<List<Usuario>> ListarUsuarios()
        {
            // Usamos AsNoTracking para mejorar el rendimiento en consultas de solo lectura
            return await _contexto.Usuarios
                .AsNoTracking()
                .OrderBy(u => u.NombreCompleto)
                .ToListAsync();
        }
    }
}