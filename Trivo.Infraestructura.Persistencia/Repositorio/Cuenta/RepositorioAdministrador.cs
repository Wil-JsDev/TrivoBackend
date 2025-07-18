using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio.Cuenta;

public class RepositorioAdministrador(TrivoContexto trivoContexto) : 
    RepositorioGenerico<Administrador>(trivoContexto), 
    IRepositorioAdministrador
{
    public async Task BanearUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        var usuario = await _trivoContexto.Set<Usuario>().FirstOrDefaultAsync(x => x.Id == usuarioId, cancellationToken);
        
        usuario!.EstadoUsuario = nameof(EstadoUsuario.Baneado);
        
        _trivoContexto.Set<Usuario>().Update(usuario);
        
        await _trivoContexto.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DesbanearUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        var usuario = await _trivoContexto.Set<Usuario>().FirstOrDefaultAsync(x => x.Id == usuarioId, cancellationToken);
        
        usuario!.EstadoUsuario = nameof(EstadoUsuario.Activo);
        
        _trivoContexto.Set<Usuario>().Update(usuario);
        
        await _trivoContexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Usuario>> ObtenerUsuariosBaneadosAsync(CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<Usuario>()
            .AsNoTracking()
            .Where(usuario =>  usuario.EstadoUsuario == nameof(EstadoUsuario.Baneado))
            .ToListAsync(cancellationToken);
    }
}