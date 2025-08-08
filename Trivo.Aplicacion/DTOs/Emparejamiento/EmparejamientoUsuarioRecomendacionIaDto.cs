using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

namespace Trivo.Aplicacion.DTOs.Emparejamiento;

public sealed record EmparejamientoUsuarioRecomendacionIaDto
(
    Guid EmparejamientoId,
    IEnumerable<UsuarioRecomendacionIaDto> UsuarioRecomendacionIaDto
);