using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

namespace Trivo.Aplicacion.Modulos.Usuario.Querys.ObtenerFotoDePerfil;

public sealed record ObtenerFotoPerfilPorUsuarioIdQuery
(
    Guid UsuarioId
) : IQuery<FotoDePerfilUsuarioDto>;