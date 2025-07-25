using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Emparejamiento;

namespace Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerConteoDeEmparejamientos;

public sealed record ObtenerConteoEmparejamientosCompletadosQuery() : IQuery<ConteoEmparejamientoCompletadosDto>;