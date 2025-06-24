using MediatR;
using Trivo.Aplicacion.Utilidades;

namespace Trivo.Aplicacion.Abstracciones.Mensajes;

public interface ICommandHandler<in TCommand> : 
    IRequestHandler<TCommand, Resultado>
    where TCommand : ICommand;

public interface ICommandHandler<in TCommand, TResponse> :
    IRequestHandler<TCommand, ResultadoT<TResponse>> 
    where TCommand : ICommand<TResponse>;
