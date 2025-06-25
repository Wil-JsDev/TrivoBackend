using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerDetalles;

public sealed record ObtenerDetallesUsuarioQuery(Guid UsuarioId) : IQuery<UsuarioDetallesDto>;