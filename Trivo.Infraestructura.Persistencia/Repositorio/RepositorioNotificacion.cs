using Trivo.Aplicacion.Interfaces.Repositorio;
using Trivo.Dominio.Modelos;
using Trivo.Infraestructura.Persistencia.Contexto;

namespace Trivo.Infraestructura.Persistencia.Repositorio;

public class RepositorioNotificacion(TrivoContexto trivoContexto) : RepositorioGenerico<Notificacion>(trivoContexto), IRepositorioNotificacion;