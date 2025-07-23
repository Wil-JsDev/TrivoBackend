using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.DTOs.Mensaje;
using Trivo.Aplicacion.DTOs.Usuario;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Paginacion;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio;

public class RepositorioMensaje(TrivoContexto trivoContexto): RepositorioGenerico<Mensaje>(trivoContexto), IRepositorioMensaje 
{
    
    public async Task<List<Mensaje>> ObtenerPorChatIdAsync(Guid chatId)
    {
        return await trivoContexto.Set<Mensaje>()
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.FechaEnvio)
            .ToListAsync();
    }

    public async Task<Mensaje?> ObtenerUltimoPorChatIdAsync(Guid chatId)
    {
        return await trivoContexto.Set<Mensaje>()
            .Where(m => m.ChatId == chatId)
            .OrderByDescending(m => m.FechaEnvio)
            .FirstOrDefaultAsync();
    }


    public async Task<Mensaje?> ObtenerUltimoMensajePorChatIdAsync(Guid chatId, CancellationToken cancellationToken)
        => await trivoContexto.Set<Mensaje>()
            .Where(ci => ci.ChatId == chatId && ci.Contenido != null)
            .OrderByDescending(m => m.FechaEnvio).FirstOrDefaultAsync(cancellationToken);

    public async Task<ResultadoPaginado<Mensaje>> ObtenerMensajePorChatIdPaginadoAsync(Guid chatId, int pagina, int tamano, CancellationToken cancellationToken)
    {
        var consulta = trivoContexto.Set<Mensaje>()
            .Where(m => m.ChatId == chatId)
            .Include(m => m.Emisor)
            .Include(m => m.Receptor)
            .OrderByDescending(m => m.FechaEnvio);

        var total = await consulta.CountAsync(cancellationToken);

        var mensajes = await consulta
            .Skip((pagina - 1) * tamano)
            .Take(tamano)
            .ToListAsync(cancellationToken);

        return new ResultadoPaginado<Mensaje>(mensajes, total, pagina, tamano);
    }




}