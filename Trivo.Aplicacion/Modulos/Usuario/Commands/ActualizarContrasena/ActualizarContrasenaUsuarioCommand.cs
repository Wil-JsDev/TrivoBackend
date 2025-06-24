using Trivo.Aplicacion.Abstracciones.Mensajes;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ActualizarContrasena;

public sealed record ActualizarContrasenaUsuarioCommand
(
    Guid? UsuarioId,
    string Codigo,
    string Contrasena,
    string ConfirmacionDeContrsena
) : ICommand<string>; 