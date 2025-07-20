using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

namespace Trivo.Aplicacion.DTOs.Emparejamiento;

public sealed record EmparejamientoDto
(
    Guid EmparejamientoId,
    IEnumerable<UsuarioReconmendacionDto> UsuarioReconmendacionDto
);