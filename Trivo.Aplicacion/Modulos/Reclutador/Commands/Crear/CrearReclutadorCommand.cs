using Trivo.Aplicacion.Abstracciones.Mensajes;
using Trivo.Aplicacion.DTOs.Cuentas.Reclutador;

namespace Trivo.Aplicacion.Modulos.Reclutador.Commands.Crear;

public record CrearReclutadorCommand(    
    string NombreEmpresa,
    Guid UsuarioId
) : ICommand<ReclutadorDto>;