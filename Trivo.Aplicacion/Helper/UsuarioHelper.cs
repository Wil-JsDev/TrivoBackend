using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Helper;

public static class UsuarioHelper
{
    public static string NombreCompleto(this Usuario usuario)
    {
        return $"{usuario.Nombre} {usuario.Apellido}".Trim();
    }
}