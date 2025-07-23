using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Serilog;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Chat;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Aplicacion.Mapper;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Chat.Querys.Paginacion;

internal class ObtenerPaginasChatQueryHandler(
    ILogger<ObtenerPaginasChatQueryHandler> logger,
    IRepositorioChat repositorioChat,
    IDistributedCache cache,
    INotificadorTiempoReal notificador
    ): IQueryHandler<ObtenerPaginasChatQuery, ResultadoPaginado<ChatDto>>
{
    public async Task<ResultadoT<ResultadoPaginado<ChatDto>>> Handle(ObtenerPaginasChatQuery request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("La solicitud para obtener chat paginadas fue nula.");
            return ResultadoT<ResultadoPaginado<ChatDto>>.Fallo(Error.Fallo("400", "La solicitud no puede ser nula."));
        }

        if (request.NumeroPagina <= 0 || request.TamanoPagina <= 0)
        {
            logger.LogWarning("Parámetros de paginación inválidos. NumeroPagina={NumeroPagina}, TamanoPagina={TamanoPagina}", 
                request.NumeroPagina, request.TamanoPagina);
            
            return ResultadoT<ResultadoPaginado<ChatDto>>.Fallo(Error.Fallo("400", "Los parámetros de paginación deben ser mayores a cero."));

        }


        var resultadoPagina = 
             await repositorioChat.ObtenerChatsPorUsuarioIdPaginadoAsync(
                request.UsuarioId,
                request.NumeroPagina,
                request.TamanoPagina,
                cancellationToken);

        var elementos = resultadoPagina.Elementos!
            .Select(chat => MapperChat.MapChatToDto(chat, request.UsuarioId)) 
            .ToList();
        
        if (!elementos.Any())
        {
            logger.LogWarning("No se encontraron chats en la página {NumeroPagina}.", request.NumeroPagina);
            
            return ResultadoT<ResultadoPaginado<ChatDto>>.Fallo(
                Error.Fallo("404", "No se encontraron chats."));
        }
        
        
        var resultadoPaginado = new ResultadoPaginado<ChatDto>(
            elementos: elementos,
            totalElementos: resultadoPagina.TotalElementos,
            paginaActual: request.NumeroPagina,
            tamanioPagina: request.TamanoPagina
        );
        
        await notificador.NotificarChatsPaginados(request.UsuarioId, resultadoPaginado);
        logger.LogInformation(
            "Se obtuvo exitosamente la página {NumeroPagina} de chats. Chats total en esta página: {CantidadActividades}",
            request.NumeroPagina, elementos.Count);
        return ResultadoT<ResultadoPaginado<ChatDto>>.Exito(resultadoPaginado);

    }
}