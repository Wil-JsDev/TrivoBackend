using Microsoft.AspNetCore.Http;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Mensaje;

namespace Trivo.Aplicacion.Modulos.Mensajes.Commands.EnviarImagen;

public record EnviarImagenCommand
(    
    Guid ChatId,
    Guid EmisorId,
    Guid ReceptorId,
    IFormFile Imagen
) : ICommand<MensajeDto>;