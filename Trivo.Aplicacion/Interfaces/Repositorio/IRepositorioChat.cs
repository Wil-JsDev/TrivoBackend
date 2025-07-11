using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Interfaces.Repositorio;

public interface IRepositorioChat : IRepositorioGenerico<Chat>
{
    Task<IEnumerable<Chat>> Obtener10ChatsRecientes(Guid usuarioId, CancellationToken cancellationToken);
}