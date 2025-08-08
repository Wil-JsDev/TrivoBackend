using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Administrador;
using Trivo.Aplicacion.Paginacion;

namespace Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerUltimosEmparejamientos;

public sealed record ObtenerUltimosEmparejamientosQuery
(
    int NumeroPagina,
    int TamanoPagina
) : IQuery<ResultadoPaginado<EmparejamientoAdministradorDto>>;