using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Intereses;

namespace Trivo.Aplicacion.Modulos.Intereses.Querys.BuscarPorNombre;

public sealed record BuscarInteresesPorNombreQuery
(
    string Nombre
) : IQuery<IEnumerable<InteresConIdDto>>;