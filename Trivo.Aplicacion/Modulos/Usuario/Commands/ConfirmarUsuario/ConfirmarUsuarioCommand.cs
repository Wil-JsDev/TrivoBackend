using Trivo.Aplicacion.Abstracciones.Mensajes;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ConfirmarUsuario;

public sealed record ConfirmarUsuarioCommand
(
    Guid UsuarioId,
    string Codigo
) : ICommand<string>;