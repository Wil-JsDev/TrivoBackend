using Microsoft.AspNetCore.Http;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.Crear;

public sealed record CrearUsuarioCommand
(
    string? Nombre, 
    string? Apellido, 
    string? Biografia,
    string? Email,
    string? Contrasena,
    string? NombreUsuario,
    string? Ubicacion,
    IFormFile? Foto
) : ICommand<UsuarioDto>;
