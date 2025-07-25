using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Expertos;

namespace Trivo.Aplicacion.Modulos.Experto.Commands.Crear;

public sealed record CrearExpertoCommand(
    Guid UsuarioId,
    bool DisponibleParaProyectos,
    bool Contratado ) : ICommand<ExpertoDto>;
