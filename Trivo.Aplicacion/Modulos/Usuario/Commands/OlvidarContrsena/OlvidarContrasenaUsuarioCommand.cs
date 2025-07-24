using Trivo.Aplicacion.Abstracciones.Mensajes;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.OlvidarContrsena;

public sealed record OlvidarContrasenaUsuarioCommand
(
    string Email
) : ICommand<string>;