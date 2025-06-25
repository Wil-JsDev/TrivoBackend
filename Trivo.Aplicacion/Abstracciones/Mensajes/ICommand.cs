using MediatR;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Abstracciones.Mensajes;

public interface ICommand : IRequest<Resultado>, IBaseCommand;

public interface ICommand<TResponse> : IRequest<ResultadoT<TResponse>>, IBaseCommand;

public interface IBaseCommand; 