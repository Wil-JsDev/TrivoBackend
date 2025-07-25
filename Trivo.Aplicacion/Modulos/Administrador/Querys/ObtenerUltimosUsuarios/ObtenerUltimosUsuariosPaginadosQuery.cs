using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.Paginacion;

namespace Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerUltimosUsuarios;

public sealed record ObtenerUltimosUsuariosPaginadosQuery
(
    int NumeroPagina,
    int TamanoPagina
) : IQuery<ResultadoPaginado<UsuarioDto>>;