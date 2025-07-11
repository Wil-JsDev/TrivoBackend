using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio;

public class RepositorioChat(TrivoContexto trivoContexto) : RepositorioGenerico<Chat>(trivoContexto), IRepositorioChat
{
    public async Task<IEnumerable<Chat>> Obtener10ChatsRecientes(Guid usuarioId, CancellationToken cancellationToken)
    {
       List<Chat?> chats = await _trivoContexto.Set<ChatUsuario>()
           .Where(cu => cu.UsuarioId == usuarioId)
           .Include(cu => cu.Chat)
           .ThenInclude(c => c!.ChatUsuarios)!
                    .ThenInclude(cu => cu.Usuario)
           .OrderByDescending(cu => cu.Chat!.Mensaje!.FechaRegistro)
           .Take(10)
           .Select(cu => cu.Chat)
           .ToListAsync(cancellationToken);
       
       return chats!;
    }
}