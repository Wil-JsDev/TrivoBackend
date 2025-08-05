using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

namespace Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerUsuariosBaneados;

public sealed record ObtenerUltimos10UsuariosBaneadosQuery
(

) : IQuery<IEnumerable<UsuarioDto>>;