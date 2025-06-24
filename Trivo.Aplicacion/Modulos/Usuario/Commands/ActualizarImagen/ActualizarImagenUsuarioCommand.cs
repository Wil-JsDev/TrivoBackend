using Microsoft.AspNetCore.Http;
using Trivo.Aplicacion.Abstracciones.Mensajes;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ActualizarImagen;

public sealed record ActualizarImagenUsuarioCommand(Guid UsuarioId, IFormFile Imagen) : ICommand<string>;