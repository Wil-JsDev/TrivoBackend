namespace Trivo.Aplicacion.DTOs.Cuentas.Expertos;

public sealed record ActualizarExpertoDto( 
    Guid Id,                       
    bool DisponibleParaProyectos,
    bool Contratado);