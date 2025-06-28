using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Intereses;
using Trivo.Aplicacion.Paginacion;

namespace Trivo.Aplicacion.Modulos.Intereses.Querys.ObtenerInteresCategoriaId;

public sealed record ObtenerInteresesPorCategoriaQuery(
    IEnumerable<Guid> CategoriaIds,
    int NumeroPagina,
    int TamanoPagina
) : IQuery<ResultadoPaginado<InterestPorCategoriaIdDto>>;
