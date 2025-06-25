using MediatR;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Abstracciones.Mensajes;

public interface IQueryHandler<in TQuery, TResponse> :
    IRequestHandler<TQuery, ResultadoT<TResponse>>
    where TQuery : IQuery<TResponse>;