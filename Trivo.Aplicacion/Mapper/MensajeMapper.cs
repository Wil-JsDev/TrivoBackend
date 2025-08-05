using Trivo.Aplicacion.DTOs.Reportes;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Mapper;

public static class MensajeMapper
{
    public static MensajeDtoParaReporte MappearAMensajeDtoParaReporte(
        Mensaje mensaje, 
        bool incluirEmisor = true) // true = Emisor, false = Receptor
    {
        if (mensaje == null) return null;

        // Selecciona el usuario relevante (Emisor o Receptor)
        var usuarioRelevante = incluirEmisor ? mensaje.Emisor : mensaje.Receptor;
        var usuarioIdRelevante = incluirEmisor ? mensaje.EmisorId : mensaje.ReceptorId;

        return new MensajeDtoParaReporte(
            mensaje.MensajeId,
            mensaje.EmisorId,
            mensaje.Contenido,
            mensaje.Tipo,
            mensaje.FechaEnvio,
            usuarioRelevante != null ? new UsuarioDtoParaReporte(
                usuarioRelevante.Id ?? Guid.Empty,
                usuarioRelevante.Nombre!,
                usuarioRelevante.Apellido!
            ) : null
        );
    }
}