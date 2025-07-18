using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Chat;
using Trivo.Aplicacion.Paginacion;

namespace Trivo.Aplicacion.Modulos.Chat.Querys.Paginacion;

public sealed record ObtenerPaginasChatQuery(
    Guid UsuarioId,
    int NumeroPagina,
    int TamanoPagina
    ): IQuery<ResultadoPaginado<ChatDto>>;