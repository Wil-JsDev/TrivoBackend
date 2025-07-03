using Microsoft.AspNetCore.Http;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.Crear;

public sealed class CrearUsuarioCommand : ICommand<UsuarioDto>
{
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Biografia { get; set; }
    public string? Email { get; set; }
    public string? Contrasena { get; set; }
    public string? NombreUsuario { get; set; }
    public string? Ubicacion { get; set; }
    
    public List<Guid>? Intereses { get; set; }
    public IFormFile? Foto { get; set; }
}