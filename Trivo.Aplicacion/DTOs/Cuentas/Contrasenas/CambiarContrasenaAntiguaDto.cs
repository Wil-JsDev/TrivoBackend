namespace Trivo.Aplicacion.DTOs.Cuentas.Contrasenas;

public sealed record CambiarContrasenaAntiguaDto(string AntiguaContrasena, string NuevaContrasena, string ConfirmacionDeContrsena);