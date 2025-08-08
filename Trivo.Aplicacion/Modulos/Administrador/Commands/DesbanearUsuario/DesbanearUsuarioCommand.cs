using Trivo.Aplicacion.Abstracciones.Mensajes;

namespace Trivo.Aplicacion.Modulos.Administrador.Commands.DesbanearUsuario;

public sealed record DesbanearUsuarioCommand
(
    Guid? UsuarioId
) : ICommand<string>;