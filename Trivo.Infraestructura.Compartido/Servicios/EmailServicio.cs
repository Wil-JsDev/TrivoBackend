using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Trivo.Aplicacion.DTOs.Email;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Dominio.Configuraciones;

namespace Trivo.Infraestructura.Compartido.Servicios;

public class EmailServicio(IOptions<EmailConfiguraciones> emailOpciones) : IEmailServicio
{
    private EmailConfiguraciones _emailConfiguraciones { get; } = emailOpciones.Value;

    public async Task EnviarEmailAsync(EmailRespuestaDto emailRespuesta)
    {
        try
        {
            MimeMessage email = new();
            email.Sender = MailboxAddress.Parse(_emailConfiguraciones.EmailFrom);
            email.To.Add(MailboxAddress.Parse(emailRespuesta.Usuario));
            email.Subject = emailRespuesta.Tema;
            BodyBuilder builder = new()
            {
                HtmlBody = emailRespuesta.Cuerpo
            };
            email.Body = builder.ToMessageBody();

            //Configuaciones SMTP
            using MailKit.Net.Smtp.SmtpClient smtp = new();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
            await smtp.ConnectAsync(_emailConfiguraciones.SmtpHost, _emailConfiguraciones.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailConfiguraciones.SmtpUser, _emailConfiguraciones.SmtpPass);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception)
        {
            // ignored
        }
    }
}