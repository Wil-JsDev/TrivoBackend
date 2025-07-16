using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Habilidades;
using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Modulos.Habilidades.Commands.Crear;

public sealed record CrearHabilidadCommand(string Nombre, Guid UsearioId) : ICommand<HabilidadDto>;