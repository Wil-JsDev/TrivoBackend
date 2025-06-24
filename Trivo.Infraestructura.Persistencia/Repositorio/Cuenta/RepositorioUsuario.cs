using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio.Cuenta;

public class RepositorioUsuario(TrivoContexto trivoContexto) : 
    RepositorioGenerico<Usuario>(trivoContexto), 
    IRepositorioUsuario
{
    
    public async Task<bool> CuentaConfirmadaAsync(Guid id, CancellationToken cancellationToken) =>
        await ValidarAsync(usuario => usuario.Id == id && usuario.CuentaConfirmada == true, cancellationToken);

    public async Task<bool> NombreUsuarioEnUso(string nombreUsuario, Guid usuarioId, CancellationToken cancellationToken)=>
        await ValidarAsync(usuario => usuario.Nombre == nombreUsuario && usuario.Id != usuarioId, cancellationToken);

    public async Task<EstadoUsuario?> ObtenerEstadoUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken) =>
        await _trivoContexto.Set<Usuario>()
            .AsNoTracking()
            .Where(usuario => usuario.Id == usuarioId)
            .Select(usuario => usuario.EstadoUsuario)
            .FirstOrDefaultAsync(cancellationToken);
            

    public async Task<Usuario> BuscarPorEmailUsuarioAsync(string email, CancellationToken cancellationToken) =>
        (await _trivoContexto.Set<Usuario>()
            .AsNoTracking()
            .Where(usuario => usuario.Email == email)
            .FirstOrDefaultAsync(cancellationToken))!;

    public async Task<Usuario> BuscarPorNombreUsuarioAsync(string nombreUsuario, CancellationToken cancellationToken) =>
        (await _trivoContexto.Set<Usuario>()
            .AsNoTracking()
            .Where(usuario => usuario.Nombre == nombreUsuario)
            .FirstOrDefaultAsync(cancellationToken))!;
    
    public async Task<List<Guid>> ObtenerHabilidadesUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken)=>
        await _trivoContexto.Set<UsuarioHabilidad>()
            .Where(uh => uh.UsuarioId == usuarioId)
            .Select(uh => uh.HabilidadId!.Value)
            .ToListAsync(cancellationToken);
    
    public async Task<List<Guid>> ObtenerInteresesUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken)=>
        await _trivoContexto.Set<UsuarioInteres>()
            .Where(uh => uh.UsuarioId == usuarioId)
            .Select(uh => uh.InteresId!.Value)
            .ToListAsync(cancellationToken);

    public async Task<bool> EmailEnUsoAsync(string email, Guid excluirUsuarioId, CancellationToken cancellationToken) =>
        await ValidarAsync(us => us.Email == email && us.Id != excluirUsuarioId, cancellationToken);

    public async Task ActualizarContrasenaAsync(Usuario usuario, string nuevaContrasena, CancellationToken cancellationToken)
    {
       usuario.ContrasenaHash = nuevaContrasena;
        _trivoContexto.Set<Usuario>().Update(usuario);
        await _trivoContexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExisteEmailAsync(string email, CancellationToken cancellationToken) =>
        await ValidarAsync(us => us.Email == email, cancellationToken);

    public async Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario, CancellationToken cancellationToken) =>
        await ValidarAsync(us => us.Nombre == nombreUsuario, cancellationToken);


    public async Task<IEnumerable<Usuario>> FiltrarPorHabilidadesAsync(List<Guid> habilidadesIds, CancellationToken cancellationToken)
    {
        var usuarioIds = await _trivoContexto.Set<UsuarioHabilidad>()
            .AsNoTracking()
            .Where(uh => habilidadesIds.Contains(uh.HabilidadId!.Value))
            .Select(uh => uh.UsuarioId)
            .Distinct()
            .ToListAsync(cancellationToken);

        return await _trivoContexto.Set<Usuario>()
            .AsNoTracking()
            .Where(u => usuarioIds.Contains(u.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Usuario>> FiltrarPorInteresesAsync(List<Guid> interesesIds, CancellationToken cancellationToken)
    {
        var usuarioIdsConIntereses = await _trivoContexto.Set<UsuarioInteres>()
            .AsNoTracking()
            .Where(ui => interesesIds.Contains(ui.InteresId!.Value))
            .Select(ui => ui.UsuarioId)
            .Distinct()
            .ToListAsync(cancellationToken);

        return await _trivoContexto.Set<Usuario>()
            .AsNoTracking()
            .Where(u => usuarioIdsConIntereses.Contains(u.Id))
            .ToListAsync(cancellationToken);
       
    }
}