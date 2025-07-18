using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Mensaje;
using Trivo.Aplicacion.Paginacion;

namespace Trivo.Aplicacion.Modulos.Mensajes.Querys;

public sealed record ObtenerPaginasMensajesQuery(
    Guid chatId,
    int NumeroPagina,
    int TamanoPagina
    ): IQuery<ResultadoPaginado<MensajeDto>>;