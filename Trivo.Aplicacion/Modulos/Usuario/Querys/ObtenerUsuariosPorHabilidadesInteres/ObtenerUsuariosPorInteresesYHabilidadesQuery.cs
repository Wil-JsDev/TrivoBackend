using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.Paginacion;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerUsuariosPorHabilidadesInteres;

public sealed record ObtenerUsuariosPorInteresesYHabilidadesQuery
(
    int NumeroPagina,
    int TamanoPagina,
    List<Guid> HabilidadesIds,
    List<Guid> InteresesIds
) : IQuery<ResultadoPaginado<UsuarioReconmendacionInteresHabilidadDto>>;