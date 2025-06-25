namespace Trivo.Aplicacion.DTOs.Cuentas.Reclutador;

public sealed record ReclutadorDto(    
    Guid Id,
    string NombreEmpresa,
    Guid UsuarioId);