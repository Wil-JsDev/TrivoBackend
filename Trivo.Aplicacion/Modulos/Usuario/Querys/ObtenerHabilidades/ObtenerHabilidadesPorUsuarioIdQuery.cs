using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Habilidades;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerHabilidades;

public sealed record ObtenerHabilidadesPorUsuarioIdQuery(Guid UsuarioId) : IQuery<IEnumerable<HabilidadConIdDto>>;