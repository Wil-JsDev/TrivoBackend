using Microsoft.AspNetCore.Http;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Mensaje;

namespace Trivo.Aplicacion.Modulos.Mensajes.Commands.EnviarArchivo;

public record EnviarArchivoCommand(
    Guid ChatId,
    Guid EmisorId,
    Guid ReceptorId,
    IFormFile Archivo
) : ICommand<MensajeDto>;