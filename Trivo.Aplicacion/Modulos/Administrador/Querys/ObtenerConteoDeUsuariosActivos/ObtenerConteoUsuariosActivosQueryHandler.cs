using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerConteoDeUsuariosActivos;

internal sealed class ObtenerConteoUsuariosActivosQueryHandler(
    ILogger<ObtenerConteoUsuariosActivosQueryHandler> logger,
    IRepositorioAdministrador repositorioAdministrador
    ) : IQueryHandler<ObtenerConteoUsuariosActivosQuery, ConteoUsuariosActivosDto>
{
    public async Task<ResultadoT<ConteoUsuariosActivosDto>> Handle(ObtenerConteoUsuariosActivosQuery request, CancellationToken cancellationToken)
    {
        var conteoUsuariosActivos =
            await repositorioAdministrador.ContarUsuariosActivosAsync(cancellationToken);

        if (conteoUsuariosActivos == 0)
        {
            logger.LogInformation("No se encontraron usuarios activos en la base de datos.");
        
            return ResultadoT<ConteoUsuariosActivosDto>.Fallo(
                Error.NoEncontrado("404", "No se encontraron usuarios activos."));
        }
    
        logger.LogInformation("Se encontraron {ConteoUsuariosActivos} usuarios activos.", conteoUsuariosActivos);
    
        return ResultadoT<ConteoUsuariosActivosDto>.Exito(new ConteoUsuariosActivosDto(conteoUsuariosActivos));
    }

}