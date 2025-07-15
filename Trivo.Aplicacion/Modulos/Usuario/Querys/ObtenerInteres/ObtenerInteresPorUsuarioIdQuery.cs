using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Intereses;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerInteres;

public sealed record ObtenerInteresPorUsuarioIdQuery(Guid UsuarioId) : IQuery<IEnumerable<InteresDto>>;