using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.JWT;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.InicioSesion;

public sealed record InicioSesionUsuarioCommand 
(
    string Email,
    string Contrasena
) : ICommand<TokenRespuestaDto>;