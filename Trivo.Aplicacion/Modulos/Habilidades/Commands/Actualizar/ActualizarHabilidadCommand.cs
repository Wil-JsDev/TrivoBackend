using Trivo.Aplicacion.Abstracciones.Mensajes;

namespace Trivo.Aplicacion.Modulos.Habilidades.Commands.Actualizar;

public sealed record ActualizarHabilidadCommand
(
    Guid UsuarioId,
    List<Guid> HabilidadIds
) : ICommand<string>;