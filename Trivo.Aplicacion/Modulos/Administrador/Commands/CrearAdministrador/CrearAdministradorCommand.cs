using Microsoft.AspNetCore.Http;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Administrador;

namespace Trivo.Aplicacion.Modulos.Administrador.Commands.CrearAdministrador;

public sealed class CrearAdministradorCommand : ICommand<AdministradorDto>
{
    public string? Nombre { get; set; }
    
    public string? Apellido { get; set; }
    
    public string? Biografia { get; set; }
    
    public string? Email { get; set; }
    
    public string? Contrasena { get; set; }

    public string? NombreUsuario { get; set; }
    
    public IFormFile? Foto { get; set; }
}