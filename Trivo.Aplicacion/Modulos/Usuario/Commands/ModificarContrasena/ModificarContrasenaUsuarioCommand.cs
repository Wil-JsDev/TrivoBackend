using Trivo.Aplicacion.Abstracciones.Mensajes;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ModificarContrasena;

public sealed record ModificarContrasenaUsuarioCommand
(
    Guid UsuarioId,
    string Codigo,
    string Contrasena,
    string ConfirmacionDeContrsena
) : ICommand<string>;