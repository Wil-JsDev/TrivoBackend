using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

namespace Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerConteoDeUsuariosActivos;

public sealed record ObtenerConteoUsuariosActivosQuery() : IQuery<ConteoUsuariosActivosDto>;