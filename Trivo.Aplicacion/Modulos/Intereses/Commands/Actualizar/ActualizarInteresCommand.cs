using Trivo.Aplicacion.Abstracciones.Mensajes;

namespace Trivo.Aplicacion.Modulos.Intereses.Commands.Actualizar;

public record ActualizarInteresCommand
(
    Guid UsuarioId,
    List<Guid> InteresIds
) : ICommand<string>;