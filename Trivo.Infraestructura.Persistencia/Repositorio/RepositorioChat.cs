using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Aplicacion.Paginacion;
using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio;

public class RepositorioChat(TrivoContexto trivoContexto) : RepositorioGenerico<Chat>(trivoContexto), IRepositorioChat
{
    public async Task<IEnumerable<Chat>> Obtener10ChatsRecientes(Guid usuarioId, CancellationToken cancellationToken)
    {
        var chats = await _trivoContexto.Set<ChatUsuario>()
            .Where(cu => cu.UsuarioId == usuarioId)
            .OrderByDescending(cu => cu.Chat!.FechaRegistro)
            .Select(cu => cu.Chat!)
            .Include(c => c.ChatUsuarios)
            .ThenInclude(cu => cu.Usuario)
            .Take(10)
            .ToListAsync(cancellationToken);

        return chats;
    }


    public async Task<ResultadoPaginado<Chat>> ObtenerChatsPorUsuarioIdPaginadoAsync(
        Guid usuarioId, int pagina, int tamano, CancellationToken cancellationToken)
    {
        var total = await trivoContexto.Set<ChatUsuario>()
            .Where(cu => cu.UsuarioId == usuarioId)
            .CountAsync(cancellationToken);

        var chats = await trivoContexto.Set<ChatUsuario>()
            .Where(cu => cu.UsuarioId == usuarioId)
            .OrderByDescending(cu => cu.FechaIngreso)
            .Skip((pagina - 1) * tamano)
            .Take(tamano)
            .Select(cu => new Chat
            {
                Id = cu.Chat.Id,
                FechaRegistro = cu.Chat.FechaRegistro,
                ChatUsuarios = cu.Chat.ChatUsuarios.Select(c => new ChatUsuario
                {
                    UsuarioId = c.UsuarioId,
                    NombreChat = c.NombreChat,
                    Usuario = new Usuario
                    {
                        Id = c.Usuario.Id,
                        NombreUsuario = c.Usuario.NombreUsuario,
                        Nombre = c.Usuario.Nombre,
                        Apellido = c.Usuario.Apellido,
                        FotoPerfil = c.Usuario.FotoPerfil
                    }
                }).ToList(),
                Mensajes = cu.Chat.Mensajes
                    .OrderByDescending(m => m.FechaEnvio)
                    .Take(1)
                    .Select(m => new Mensaje
                    {
                        MensajeId = m.MensajeId,
                        Contenido = m.Contenido,
                        FechaEnvio = m.FechaEnvio,
                        Estado = m.Estado,
                        EmisorId = m.EmisorId,
                        ChatId = m.ChatId
                    }).ToList()
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new ResultadoPaginado<Chat>(chats, total, pagina, tamano);
    }

    public async Task<Chat> ObtenerChatPorIdAsync(Guid chatId, CancellationToken cancellationToken) 
        => await trivoContexto.Set<Chat>()
            .Include(c => c.ChatUsuarios)
            .ThenInclude(cu => cu.Usuario)
            .Include(c => c.Mensajes)
            .Where(c => c.Id == chatId)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<bool> UsuarioPerteneceAlChatAsync(Guid chatId, Guid usuarioId, CancellationToken cancellationToken)
        => await trivoContexto.Set<ChatUsuario>()
            .AnyAsync(c => c.ChatId == chatId && c.UsuarioId == usuarioId, cancellationToken);
    
    public async Task<bool> ExisteAsync(Guid chatId,CancellationToken cancellationToken) 
        => await trivoContexto.Set<ChatUsuario>()
            .AnyAsync(c => c.ChatId == chatId);
   
    public async Task<Chat?> BuscarChat1a1Async(Guid usuario1Id, Guid usuario2Id, CancellationToken cancellationToken)
    {
        return await trivoContexto.Chat
            .Include(c => c.ChatUsuarios)
            .ThenInclude(cu => cu.Usuario)
            .Include(c => c.Mensajes)
            .ThenInclude(m => m.Usuario) 
            .Where(c => c.TipoChat == TipoChat.Privado.ToString())
            .Where(c => c.ChatUsuarios.Any(cu => cu.UsuarioId == usuario1Id) &&
                        c.ChatUsuarios.Any(cu => cu.UsuarioId == usuario2Id))
            .OrderByDescending(c => c.Mensajes.Max(m => m.FechaEnvio)) 
            .FirstOrDefaultAsync(cancellationToken);
    }
    
}