using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Reportes;
using Trivo.Aplicacion.Paginacion;

namespace Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerUsuariosBaneados;

public sealed record ObtenerUsuariosBaneadosConPaginacionQuery
(
    int NumeroPagina,
    int TamanoPagina
) : IQuery<ResultadoPaginado<ReporteBanDto>>;