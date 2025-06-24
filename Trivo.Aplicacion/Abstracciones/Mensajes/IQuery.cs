using MediatR;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Abstracciones.Mensajes;

public interface IQuery<TResponse> : IRequest<ResultadoT<TResponse>>;