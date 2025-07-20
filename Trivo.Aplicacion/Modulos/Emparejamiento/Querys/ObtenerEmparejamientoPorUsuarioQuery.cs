using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Emparejamiento;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Emparejamiento.Querys;

public sealed record ObtenerEmparejamientoPorUsuarioQuery
(
    Guid UsuarioId,
    Roles Rol
) : IQuery<IEnumerable<EmparejamientoDto>>;