namespace Trivo.Aplicacion.DTOs.Cuentas.Expertos;

public sealed record ExpertoDto(   
    Guid Id,
    Guid UsuarioId,
    bool DisponibleParaProyectos,
    bool Contratado);