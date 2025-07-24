using Trivo.Aplicacion.Abstracciones.Mensajes;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ActualizarContrasenaAntigua;

public sealed record ActualizarContrasenaAntiguaUsuarioCommand
(
    Guid UsuarioId,
    string ContrasenaAntigua,
    string NuevaContrasena,
    string ConfirmacionDeContrsena
) : ICommand<string>;