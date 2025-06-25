using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Reclutador;

namespace Trivo.Aplicacion.Modulos.Reclutador.Commands.Actualizar;

public sealed record ActualizarReclutadorCommand(
    Guid ReclutadorId,
    string NombreEmpresa
) : ICommand<ReclutadorDto>;