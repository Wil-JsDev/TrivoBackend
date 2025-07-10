using Microsoft.EntityFrameworkCore;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Servicios;

public class ObtenerReclutadorIdServicio(TrivoContexto trivoContexto) : IObtenerReclutadorIdServicio
{
    public async Task<Guid?> ObtenerReclutadorIdAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        var reclutador = await trivoContexto.Set<Reclutador>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.UsuarioId == usuarioId, cancellationToken);
        
        return reclutador!.Id;
    }
}