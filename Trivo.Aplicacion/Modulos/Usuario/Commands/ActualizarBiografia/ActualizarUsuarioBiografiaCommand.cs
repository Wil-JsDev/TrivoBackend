using Trivo.Aplicacion.Abstracciones.Mensajes;

namespace Trivo.Aplicacion.Modulos.Usuario.Commands.ActualizarBiografia;

public sealed record ActualizarUsuarioBiografiaCommand
(
    Guid UsuarioId,
    string Biografia
) : IQuery<string>;