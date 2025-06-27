using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Intereses;

namespace Trivo.Aplicacion.Modulos.Intereses.Commands.Crear;

public sealed record CrearInteresCommand
(
    string Nombre,
    Guid? CategoriaId,
    Guid? CreadoPor
) : ICommand<InteresDetallesDto>;