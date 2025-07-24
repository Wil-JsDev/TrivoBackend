using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Paginacion;
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
   public async Task<ResultadoPaginado<Usuario>> ObtenerPaginadoUsuariosBaneadosAsync(
       int numeroPagina,
       int tamanoPagina,
       CancellationToken cancellationToken)
   {
       var consulta = _trivoContexto.Set<Usuario>()
           .AsNoTracking()
           .Where(usuario => usuario.EstadoUsuario == nameof(EstadoUsuario.Baneado))
           .OrderByDescending(x => x.FechaRegistro);
       
       var total = await consulta.CountAsync(cancellationToken);
       
       var usuariosBaneados = await consulta
           .Skip((numeroPagina - 1) * tamanoPagina)
           .Take(tamanoPagina)
           .ToListAsync(cancellationToken);
       
       return new ResultadoPaginado<Usuario>(usuariosBaneados, numeroPagina, tamanoPagina, total);
   }
   
   public async Task<ResultadoPaginado<Usuario>> ObtenerPaginadoUltimosUsuariosAsync(
       int numeroPagina,
       int tamanoPagina,
       CancellationToken cancellationToken)
   {
       var consulta = _trivoContexto.Set<Usuario>()
           .AsNoTracking()
           .OrderByDescending(x => x.FechaRegistro);

       var total = await consulta.CountAsync(cancellationToken);

       var usuarios = await consulta
           .Skip((numeroPagina - 1) * tamanoPagina)
           .Take(tamanoPagina)
           .ToListAsync(cancellationToken);

       return new ResultadoPaginado<Usuario>(usuarios, total, numeroPagina, tamanoPagina);
   }

   public async Task<ResultadoPaginado<Emparejamiento>> ObtenerPaginadoUltimosEmparejamientosAsync(
       int numeroPagina,
       int tamanoPagina,
       CancellationToken cancellationToken)
   {
       var consulta = _trivoContexto.Set<Emparejamiento>()
           .AsNoTracking()
           .OrderByDescending(x => x.FechaRegistro);
       
       var total = await consulta.CountAsync(cancellationToken);
       
       var emparejamientos = await consulta
           .Skip((numeroPagina - 1) * tamanoPagina)
           .Take(tamanoPagina)
           .ToListAsync(cancellationToken);
       
       return new ResultadoPaginado<Emparejamiento>(emparejamientos, numeroPagina, tamanoPagina, total);
   }

   public Task<int> ContarEmparejamientosCompletadosAsync(CancellationToken cancellationToken)
   {
       return Task.FromResult(_trivoContexto
           .Set<Emparejamiento>()
           .AsNoTracking()
           .Count(x => x.EmparejamientoEstado == EmparejamientoEstado.Completado.ToString()));
   }
   
   public async Task<int> ContarUsuariosActivosAsync(
       CancellationToken cancellationToken)
   {
       return await _trivoContexto.Set<Usuario>()
           .AsNoTracking()
           .Where(x => x.EstadoUsuario == nameof(EstadoUsuario.Activo))
           .CountAsync(cancellationToken);
   }
   
}