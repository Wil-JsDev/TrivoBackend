using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Chat;
using Trivo.Aplicacion.DTOs.Mensaje;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Mapper;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Mensajes.Querys;

internal class ObtenerPaginasMensajesQueryHandler(
    ILogger<ObtenerPaginasMensajesQueryHandler> logger,
    IRepositorioMensaje repositorioMensaje,
    IDistributedCache cache
    ): IQueryHandler<ObtenerPaginasMensajesQuery, ResultadoPaginado<MensajeDto>>
{
    public async Task<ResultadoT<ResultadoPaginado<MensajeDto>>> Handle(ObtenerPaginasMensajesQuery request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("");
            return ResultadoT<ResultadoPaginado<MensajeDto>>.Fallo(Error.Fallo("", ""));
        }

        if (request.NumeroPagina <= 0 || request.TamanoPagina <= 0)
        {
            logger.LogWarning("Parámetros de paginación inválidos. NumeroPagina={NumeroPagina}, TamanoPagina={TamanoPagina}", 
                request.NumeroPagina, request.TamanoPagina);
            
            return ResultadoT<ResultadoPaginado<MensajeDto>>.Fallo(Error.Fallo("400", "Los parámetros de paginación deben ser mayores a cero."));

        }
        
        string cacheKey = $"obtener-paginas-mensaje-{request.chatId}-{request.NumeroPagina}-{request.TamanoPagina}";

        var resultadoPaginado = await cache.ObtenerOCrearAsync(
            cacheKey,
            async () => await repositorioMensaje.ObtenerMensajePorChatIdPaginadoAsync(
                request.chatId,
                request.NumeroPagina,
                request.TamanoPagina,
                cancellationToken)
        );

        var elementos = resultadoPaginado.Elementos!
            .Select(x => new MensajeDto(
                x.MensajeId.Value,
                x.ChatId.Value,
                x.EmisorId.Value,
                x.Estado,
                x.Contenido,
                x.FechaEnvio.Value
                
            ));
        
        if (!elementos.Any())
        {
            logger.LogWarning("No se encontraron mensajes en la página {NumeroPagina}.", request.NumeroPagina);
            
            return ResultadoT<ResultadoPaginado<MensajeDto>>.Fallo(
                Error.Fallo("404", "No se encontraron mensajes."));
        }
        
        var resultado = new ResultadoPaginado<MensajeDto>(
            elementos: elementos,
            totalElementos: resultadoPaginado.TotalElementos,
            paginaActual: request.NumeroPagina,
            tamanioPagina: request.TamanoPagina
        );
        
        logger.LogInformation("Página {NumeroPagina} de mensajes obtenida exitosamente. Total de elementos: {Cantidad}",
            request.NumeroPagina, elementos.Count());

        return ResultadoT<ResultadoPaginado<MensajeDto>>.Exito(resultado);
    }
}