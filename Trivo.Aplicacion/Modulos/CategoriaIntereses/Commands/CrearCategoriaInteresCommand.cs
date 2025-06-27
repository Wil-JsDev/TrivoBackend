using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.CategoriaIntereses;

namespace Trivo.Aplicacion.Modulos.CategoriaIntereses.Commands;

public sealed record CrearCategoriaInteresCommand(string Nombre) : ICommand<CategoriaInteresDto>;