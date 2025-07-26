using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Intereses;
using Trivo.Aplicacion.Paginacion;

namespace Trivo.Aplicacion.Modulos.Intereses.Querys.Paginacion;

public sealed record PaginacionInteresQuery
(
    int NumeroPagina,
    int TamanoPagina
) : IQuery<ResultadoPaginado<InteresDto>>;