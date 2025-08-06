using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio.Cuenta;

public class RepositorioReporte(TrivoContexto trivoContexto) : RepositorioGenerico<Reporte>(trivoContexto), IRepositorioReporte
{
    
}