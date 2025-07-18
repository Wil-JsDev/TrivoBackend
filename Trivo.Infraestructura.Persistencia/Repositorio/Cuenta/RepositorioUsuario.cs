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

    public async Task<string?> ObtenerEstadoUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken) =>
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

    public async Task<Usuario> ObtenerDetallesUsuarioPorIdAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        return (await _trivoContexto.Set<Usuario>()
            .AsNoTracking()
            .Where(u => u.Id == usuarioId)
            .Include(u => u.UsuarioHabilidades)!
            .ThenInclude(uh => uh.Habilidad)
            .Include(u => u.UsuarioInteres)!
            .ThenInclude(ui => ui.Interes)
            .Select(u => new Usuario
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Ubicacion = u.Ubicacion,
                Biografia = u.Biografia,
                FotoPerfil = u.FotoPerfil,

                UsuarioHabilidades = u.UsuarioHabilidades!
                    .Select(uh => new UsuarioHabilidad
                    {
                        HabilidadId = uh.HabilidadId, // ✅ Incluido para que no llegue como Guid.Empty
                        Habilidad = new Habilidad
                        {
                            HabilidadId = uh.HabilidadId, // opcional pero recomendable si se usa luego
                            Nombre = uh.Habilidad!.Nombre
                        }
                    }).ToList(),

                UsuarioInteres = u.UsuarioInteres!
                    .Select(ui => new UsuarioInteres
                    {
                        InteresId = ui.InteresId, // ✅ Incluido para que no llegue como Guid.Empty
                        Interes = new Interes
                        {
                            Nombre = ui.Interes!.Nombre
                        }
                    }).ToList()
            })
            .AsSingleQuery()
            .FirstOrDefaultAsync(cancellationToken))!;
    }


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
    public async Task<Usuario?> ObtenerUsuarioConInteresYHabilidades(Guid usuarioId, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<Usuario>()
            .AsNoTracking()
            .Include(u => u.UsuarioInteres)!
                .ThenInclude(ui => ui.Interes)
            .Include(u => u.UsuarioHabilidades)!
                .ThenInclude(uh => uh.Habilidad)
            .AsSplitQuery()
            .FirstOrDefaultAsync(u => u.Id == usuarioId, cancellationToken);
    }
    
    public async Task<IEnumerable<Usuario>> ObtenerTodosUsuariosConInteresesYHabilidades(CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<Usuario>()
            .AsNoTracking()
            .Include(u => u.UsuarioInteres)!
                .ThenInclude(ui => ui.Interes)
            .Include(u => u.UsuarioHabilidades)!
                .ThenInclude(uh => uh.Habilidad)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<UsuarioInteres>> ObtenerInteresesPorUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<UsuarioInteres>()
            .Where(ui => ui.UsuarioId == usuarioId)
            .Include(ui => ui.Interes)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<UsuarioHabilidad>> ObtenerHabilidadesPorUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        return await _trivoContexto.Set<UsuarioHabilidad>()
            .Where(ui => ui.UsuarioId == usuarioId)
            .Include(ui => ui.Habilidad)
            .ToListAsync(cancellationToken);
    }

    public async Task<string> ObtenerRolDeUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        var usuario = await _trivoContexto.Set<Usuario>()
            .AsNoTracking()
            .Include(u => u.Expertos)
            .Include(u => u.Reclutadores)
            .FirstOrDefaultAsync(u => u.Id == usuarioId, cancellationToken);
        
        var tieneExperto = usuario!.Expertos != null & usuario.Expertos!.Any();
        var tieneReclutador = usuario.Reclutadores != null & usuario.Reclutadores!.Any();

        if (tieneExperto)
            return nameof(Roles.Experto);
        
        if (tieneReclutador)
            return nameof(Roles.Reclutador);
        
        return "Sin Rol";
    }
    
    public async Task<IEnumerable<Usuario>> ObtenerUsuariosObjetivoAsync(Guid usuarioActualId, string rol, CancellationToken cancellationToken)
    {
        var query = _trivoContexto.Set<Usuario>()
            .AsNoTracking()
            .Include(u => u.UsuarioInteres)!.ThenInclude(ui => ui.Interes)
            .Include(u => u.UsuarioHabilidades)!.ThenInclude(uh => uh.Habilidad)
            .Include(u => u.Expertos)
            .Include(u => u.Reclutadores)
            .Where(u => u.Id != usuarioActualId); // Excluir usuario actual

        if (rol == nameof(Roles.Experto))
        {
            // Si el usuario actual es experto => devolver usuarios que TIENEN RELACIÓN con él como reclutador
            query = query.Where(u => u.Reclutadores!.Any());
        }
        else if (rol == nameof(Roles.Reclutador))
        {
            // Si el usuario actual es reclutador => devolver usuarios que TIENEN RELACIÓN con él como experto
            query = query.Where(u => u.Expertos!.Any());
        }

        return await query.AsSplitQuery().ToListAsync(cancellationToken);
    }
    
}