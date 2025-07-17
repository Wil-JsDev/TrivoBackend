using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.DTOs.Mensaje;
using Trivo.Aplicacion.DTOs.Usuario;

namespace Trivo.Aplicacion.DTOs.Chat;

public sealed record ChatDto(    
    Guid Id,
    List<UsuarioChatDto> Participantes,
    DateTime FechaCreacion,
    string Nombre,
    MensajeDto? UltimoMensaje
    );