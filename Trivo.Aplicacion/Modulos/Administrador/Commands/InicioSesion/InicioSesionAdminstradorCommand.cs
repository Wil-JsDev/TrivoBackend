using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.JWT;

namespace Trivo.Aplicacion.Modulos.Administrador.Commands.InicioSesion;

public sealed record InicioSesionAdminstradorCommand
(
    string Email,
    string Contrasena
) : ICommand<TokenRespuestaDto>;