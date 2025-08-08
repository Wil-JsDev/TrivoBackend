using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Reportes;

namespace Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerConteoUsuariosReportados;

public sealed record ObtenerConteoDeUsuariosReportadosQuery
(
) : IQuery<ConteoUsuariosReportados>;