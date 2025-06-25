using Trivo.Aplicacion.DTOs.Email;

namespace Trivo.Aplicacion.Interfaces.Servicios;

public interface IEmailServicio
{
    Task EnviarEmailAsync(EmailRespuestaDto  emailRespuesta);
}