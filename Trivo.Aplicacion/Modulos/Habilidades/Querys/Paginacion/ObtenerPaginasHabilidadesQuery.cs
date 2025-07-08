using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Aplicacion.Paginacion;

namespace Trivo.Aplicacion.Modulos.Habilidades.Querys.Paginacion;

public sealed record ObtenerPaginasHabilidadesQuery
(
    int NumeroPagina,
    int TamanoPagina
) : IQuery<ResultadoPaginado<HabilidadDto>>;