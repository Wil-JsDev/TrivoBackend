using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerRecomendacionUsuarios;

public sealed record RecomendacionUsuariosQuery
(
    Guid UsuarioId
) : IQuery<IEnumerable<UsuarioReconmendacionDto>>;