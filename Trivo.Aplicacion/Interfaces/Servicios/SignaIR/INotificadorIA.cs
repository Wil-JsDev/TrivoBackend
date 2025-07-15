using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

namespace Trivo.Aplicacion.Interfaces.Servicios.SignaIR;

public interface INotificadorIA
{
    Task NotificarRecomendaciones(Guid usuarioId, IEnumerable<UsuarioReconmendacionDto> recomendaciones);
}