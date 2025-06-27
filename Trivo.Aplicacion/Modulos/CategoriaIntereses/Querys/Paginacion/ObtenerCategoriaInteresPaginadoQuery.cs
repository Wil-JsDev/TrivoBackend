using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.CategoriaIntereses;
using Trivo.Aplicacion.Paginacion;

namespace Trivo.Aplicacion.Modulos.CategoriaIntereses.Querys.Paginacion;

public sealed record ObtenerCategoriaInteresPaginadoQuery
(
    int NumeroPagina,
    int TamanoPagina
) : IQuery<ResultadoPaginado<CategoriaInteresDto>>;