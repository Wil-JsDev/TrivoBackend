using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Expertos;

namespace Trivo.Aplicacion.Modulos.Experto.Commands.Actualizar;

public sealed record ActualizarExpertoCommand(   
    Guid ExpertoId,
    bool DisponibleParaProyectos,
    bool Contratado 
    ): ICommand<ExpertoDto>;