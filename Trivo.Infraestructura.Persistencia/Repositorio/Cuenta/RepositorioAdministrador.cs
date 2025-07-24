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
    
    public async Task<bool> NombreUsuarioEnUso(string nombreUsuario, Guid usuarioId, CancellationToken cancellationToken) =>
        await ValidarAsync(usuario => usuario.Nombre == nombreUsuario && usuario.Id != usuarioId, cancellationToken);
    
    public async Task<Administrador> BuscarPorEmailAsync(string email, CancellationToken cancellationToken) =>
        (await _trivoContexto.Set<Administrador>()
            .AsNoTracking()
            .Where(usuario => usuario.Email == email)
            .FirstOrDefaultAsync(cancellationToken))!;
    
    public async Task<bool> EmailEnUsoAsync(string email, Guid excluirUsuarioId, CancellationToken cancellationToken) =>
        await ValidarAsync(us => us.Email == email && us.Id != excluirUsuarioId, cancellationToken);
    
    public async Task<bool> ExisteEmailAsync(string email, CancellationToken cancellationToken) =>
        await ValidarAsync(us => us.Email == email, cancellationToken);

    public async Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario, CancellationToken cancellationToken)
    {
       return await ValidarAsync(us => us.Nombre == nombreUsuario, cancellationToken);
    }

    public async Task ActualizarContrasenaAsync(Administrador admin, string nuevaContrasena)
    {
        admin.ContrasenaHash = nuevaContrasena;
        _trivoContexto.Set<Administrador>().Update(admin);
        await _trivoContexto.SaveChangesAsync();
    }
}