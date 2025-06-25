using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.Actualizar;

public sealed record ActualizarUsuarioCommand
(
    Guid UsuarioId,
    string NombreUsuario,
    string Email
    
) : ICommand<ActualizarUsuarioDto>;
