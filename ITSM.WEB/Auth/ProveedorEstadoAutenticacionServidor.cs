using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ITSM.WEB.Auth;

/// <summary>
/// Proveedor de estado de autenticación para el servidor.
/// Utilizado durante el pre-renderizado para evitar errores de dependencias.
/// </summary>
public class ProveedorEstadoAutenticacionServidor : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Retorna un usuario anónimo durante el pre-renderizado en el servidor
        // La autenticación real se manejará en el cliente (WebAssembly)
        var usuarioAnonimo = new ClaimsPrincipal(new ClaimsIdentity());
        return Task.FromResult(new AuthenticationState(usuarioAnonimo));
    }
}
