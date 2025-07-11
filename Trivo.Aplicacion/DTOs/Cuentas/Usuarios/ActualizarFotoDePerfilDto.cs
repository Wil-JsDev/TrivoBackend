using Microsoft.AspNetCore.Http;

namespace Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

public sealed record ActualizarFotoDePerfilDto(IFormFile FotoPerfil);