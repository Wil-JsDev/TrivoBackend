using Trivo.Aplicacion.Abstracciones.Mensajes;

namespace Trivo.Aplicacion.Modulos.Administrador.Commands.BanearUsuarios;

public record BanearUsuariosCommand
(
    Guid UsuarioId
) : ICommand<string>;