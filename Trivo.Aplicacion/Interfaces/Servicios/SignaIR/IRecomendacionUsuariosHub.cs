using Trivo.Aplicacion.DTOs.Cuentas.Usuarios;

namespace Trivo.Aplicacion.Interfaces.Servicios.SignaIR;

public interface IRecomendacionUsuariosHub
{
    Task RecibirRecomendaciones(IEnumerable<UsuarioRecomendacionIaDto>? recomendaciones);
    
    Task NotificarNuevaRecomendacion(IEnumerable<UsuarioRecomendacionIaDto>? recomendaciones);
}