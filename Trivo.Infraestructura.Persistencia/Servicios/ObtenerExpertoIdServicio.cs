using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Servicios;

public class ObtenerExpertoIdServicio( TrivoContexto trivoContexto ) : IObtenerExpertoIdServicio
{
    public async Task<Guid?> ObtenerExpertoIdAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        var experto = await trivoContexto.Set<Experto>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.UsuarioId == usuarioId, cancellationToken);
        
        return experto?.Id;
    }
}