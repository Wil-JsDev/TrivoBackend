using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Chat;
using Trivo.Aplicacion.DTOs.Mensaje;
using Trivo.Aplicacion.DTOs.Usuario;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Interfaces.Servicios.SignaIR;
using Trivo.Aplicacion.Mapper;
using Trivo.Aplicacion.Paginacion;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Modulos.Mensajes.Querys;

internal class ObtenerPaginasMensajesQueryHandler(
    ILogger<ObtenerPaginasMensajesQueryHandler> logger,
    IRepositorioMensaje repositorioMensaje,
    IRepositorioChat repositorioChat,
    IDistributedCache cache,
    INotificadorTiempoReal notificador
    ): IQueryHandler<ObtenerPaginasMensajesQuery, ResultadoPaginado<MensajeDto>>
{
    public async Task<ResultadoT<ResultadoPaginado<MensajeDto>>> Handle(ObtenerPaginasMensajesQuery request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("la solicitud no puede estar vacia");
            return ResultadoT<ResultadoPaginado<MensajeDto>>.Fallo(Error.Fallo("", ""));
        }

        if (request.NumeroPagina <= 0 || request.TamanoPagina <= 0)
        {
            logger.LogWarning("Parámetros de paginación inválidos. NumeroPagina={NumeroPagina}, TamanoPagina={TamanoPagina}", 
                request.NumeroPagina, request.TamanoPagina);
            
            return ResultadoT<ResultadoPaginado<MensajeDto>>.Fallo(Error.Fallo("400", "Los parámetros de paginación deben ser mayores a cero."));

        }
        

        var resultadoPaginado = await repositorioMensaje.ObtenerMensajePorChatIdPaginadoAsync(
            request.ChatId,
            request.NumeroPagina,
            request.TamanoPagina,
            cancellationToken);

        var elementos = resultadoPaginado.Elementos!
            .Select(x => x with { MensajeId = x.MensajeId!, ChatId = x.ChatId!, Contenido = x.Contenido! });
        
        
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

        var chat = await repositorioChat.ObtenerChatConUsuariosYMensajesAsync(request.ChatId, cancellationToken);
        if (chat is null)
        {
            logger.LogWarning("Chat no encontrado para notificacion de paginacion");
            return ResultadoT<ResultadoPaginado<MensajeDto>>.Fallo(Error.Fallo("404", "Chat no encontrado"));
        }

        var usuarios = chat.ChatUsuarios.Select(cu => cu.UsuarioId!.Value).ToList();

        foreach(var usuarioId in usuarios)
        {
            await notificador.NotificarPaginaMensajes(usuarioId, request.ChatId, resultado.Elementos!);
        }
        
        
        logger.LogInformation("Página {NumeroPagina} de mensajes obtenida exitosamente. Total de elementos: {Cantidad}",
            request.NumeroPagina, elementos.Count());

        return ResultadoT<ResultadoPaginado<MensajeDto>>.Exito(resultado);
    }
}