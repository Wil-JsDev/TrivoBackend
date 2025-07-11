using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Habilidades;

namespace Trivo.Aplicacion.Modulos.Habilidades.Querys.BuscarPorNombre;

public record BuscarHabilidadesPorNombreQuery
(
    string Nombre
) : IQuery<IEnumerable<HabilidadConIdDto>>;