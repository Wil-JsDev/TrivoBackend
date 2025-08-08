using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.Paginacion;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerRecomendacionUsuarios;

public sealed record RecomendacionUsuariosQuery
(
    Guid UsuarioId,
    int NumeroPagina,
    int TamanoPagina
) : IQuery<ResultadoPaginado<UsuarioRecomendacionIaDto>>;