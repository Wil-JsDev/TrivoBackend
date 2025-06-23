using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.DTOs.Email;
using Trivo.Aplicacion.Interfaces.Repositorio.Cuenta;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Aplicacion.Utilidades;
using Trivo.Dominio.Modelos;

namespace Trivo.Aplicacion.Servicios;

public class CodigoServicio(
    ILogger<CodigoServicio> logger,
    IRepositorioUsuario repositorioUsuario,
    IRepositorioCodigo repositorioCodigo
    ) : ICodigoServicio
{
    public async Task<ResultadoT<string>> GenerarCodigoAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        var usuario = await repositorioUsuario.ObtenerByIdAsync(usuarioId, cancellationToken);
        if (usuario == null)
        {
            logger.LogWarning("No se encontró ningún usuario con el ID proporcionado: {UsuarioId}", usuarioId);
    
            return ResultadoT<string>.Fallo(Error.NoEncontrado("404", "Usuario no encontrado"));
        }

        if (usuario.CuentaConfirmada!.Value)
        {
            logger.LogWarning("El usuario con ID {UsuarioId} ya tiene su cuenta confirmada.", usuarioId);
    
            return ResultadoT<string>.Fallo(Error.Conflicto("409", "La cuenta ya ha sido confirmada previamente."));
        }

        string codigoGenerado = CodigoGenerador.GenerarCodigoNumerico();

        Codigo codigo = new()
        {
            UsuarioId = usuario.Id,
            Valor = codigoGenerado,
            Expiracion = DateTime.UtcNow.AddMinutes(10)
        };

        await repositorioCodigo.CrearCodigoAsync(codigo, cancellationToken);

        logger.LogInformation("Se generó y guardó correctamente un nuevo código de verificación para el usuario con ID {UsuarioId}", usuario.Id);

        return ResultadoT<string>.Exito(codigo.Valor);

    }

    public async Task<ResultadoT<CodigoDto>> ObtenerCodigoAsync(Guid codigoId, CancellationToken cancellationToken)
    {
        var codigo = await repositorioCodigo.ObtenerCodigoPorIdAsync(codigoId, cancellationToken);
        if (codigo == null)
        {
            logger.LogWarning("No se encontró ningún código con el ID proporcionado: {CodigoId}", codigoId);
    
            return ResultadoT<CodigoDto>.Fallo(Error.NoEncontrado("404", "Código no encontrado"));
        }

        CodigoDto codigoDto = new
        (
            CodigoId: Guid.NewGuid(), 
            UsuarioId: codigo.UsuarioId ?? Guid.Empty,
            Codigo: codigo.Valor!,
            Usado: codigo.Usado!.Value,
            Expiracion: codigo.Expiracion
        );

        logger.LogInformation("Se obtuvo correctamente el código con ID {CodigoId} para el usuario {UsuarioId}", codigoId, codigo.UsuarioId);

        return ResultadoT<CodigoDto>.Exito(codigoDto);

    }

    public async Task<Resultado> BorrarCodigoAsync(Guid codigoId, CancellationToken cancellationToken)
    {
        var codigo = await repositorioCodigo.ObtenerCodigoPorIdAsync(codigoId, cancellationToken);
        if (codigo == null)
        {
            logger.LogWarning("No se encontró ningún código con el ID proporcionado: {CodigoId}", codigoId);
    
            return ResultadoT<CodigoDto>.Fallo(Error.NoEncontrado("404", "Código no encontrado"));
        }

        await repositorioCodigo.EliminarCodigoAsync(codigo, cancellationToken);

        logger.LogInformation("Se eliminó correctamente el código con ID {CodigoId} del usuario {UsuarioId}", codigo.CodigoId, codigo.UsuarioId);

        return Resultado.Exito();

    }

    public async Task<Resultado> ConfirmarCuentaAsync(Guid usuarioId, string codigo, CancellationToken cancellationToken)
    {
        var usuario = await repositorioUsuario.ObtenerByIdAsync(usuarioId, cancellationToken);
        if (usuario == null)
        {
            logger.LogWarning("No se encontró ningún usuario con el ID proporcionado: {UsuarioId}", usuarioId);
            
            return Resultado.Fallo(Error.NoEncontrado("404", "Usuario no encontrado"));
        }

        var codigoEntidad = await repositorioCodigo.BuscarCodigoAsync(codigo, cancellationToken);
        if (codigoEntidad == null)
        {
            logger.LogWarning("No se encontró ningún código con el valor: {Codigo}", codigo);
            
            return Resultado.Fallo(Error.NoEncontrado("404", "Código no encontrado"));
        }

        if (codigoEntidad.UsuarioId != usuario.Id)
        {
            logger.LogWarning("El código con valor {Codigo} no pertenece al usuario con ID {UsuarioId}", codigo, usuarioId);
            
            return Resultado.Fallo(Error.NoEncontrado("403", "El código no corresponde a este usuario"));
        }

        if (codigoEntidad.Usado is true)
        {
            logger.LogWarning("El código con valor {Codigo} ya ha sido utilizado anteriormente", codigo);
            
            return Resultado.Fallo(Error.NoEncontrado("400", "Este código ya ha sido usado"));
        }

        var esValido = await repositorioCodigo.ElCodigoEsValidoAsync(codigo, cancellationToken);
        if (!esValido)
        {
            logger.LogWarning("El código con valor {Codigo} ha expirado o no es válido", codigo);
            
            return Resultado.Fallo(Error.Fallo("400", "El código ha expirado o no es válido"));
        }

        await repositorioCodigo.MarcarCodigoComoUsado(codigoEntidad.Valor!, cancellationToken);

        usuario.CuentaConfirmada = true;
        await repositorioUsuario.ActualizarAsync(usuario, cancellationToken);

        logger.LogInformation("El usuario con ID {UsuarioId} ha confirmado su cuenta correctamente", usuarioId);
        
        return Resultado.Exito();
    }

    public async Task<Resultado> CodigoDisponibleAsync(string codigo, CancellationToken cancellationToken)
    {
        var codigoUsado = await repositorioCodigo.CodigoNoUsadoAsync(codigo, cancellationToken);
    
        if (codigoUsado)
        {
            logger.LogWarning("El código '{Codigo}' ya ha sido utilizado anteriormente.", codigo);
            
            return Resultado.Fallo(Error.Conflicto("409", "El código ya ha sido usado"));
        }

        logger.LogInformation("El código '{Codigo}' está disponible para su uso.", codigo);
        
        return Resultado.Exito();
    }

}