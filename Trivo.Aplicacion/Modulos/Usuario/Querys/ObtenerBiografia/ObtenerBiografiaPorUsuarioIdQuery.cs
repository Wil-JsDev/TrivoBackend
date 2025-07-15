using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerBiografia;

public sealed record ObtenerBiografiaPorUsuarioIdQuery(Guid UsuarioId) : IQuery<BiografiaUsuarioDto>;