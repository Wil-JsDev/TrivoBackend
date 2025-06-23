using System.Security.Cryptography;

namespace Trivo.Aplicacion.Utilidades;

public static class CodigoGenerador
{
    /// <summary>
    /// Genera un token numérico aleatorio con la cantidad de dígitos especificada.
    /// </summary>
    /// <param name="digitos">Cantidad de dígitos del token (por defecto 6). Debe estar entre 1 y 9.</param>
    /// <returns>Un token numérico como cadena.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Se lanza si el número de dígitos está fuera del rango permitido.</exception>
    public static string GenerarCodigoNumerico(int digitos = 6)
    {
        if (digitos is <= 0 or > 9)
            throw new ArgumentOutOfRangeException(nameof(digitos), "Los dígitos deben estar entre 1 y 9.");

        var maximo = (int)Math.Pow(10, digitos); // Ej: 1000000 para 6 dígitos
        var minimo = (int)Math.Pow(10, digitos - 1); // Ej: 100000 para 6 dígitos

        using var generador = RandomNumberGenerator.Create();
        var bytes = new byte[4]; // int = 4 bytes

        int numero;
        do
        {
            generador.GetBytes(bytes);
            numero = BitConverter.ToInt32(bytes, 0) & int.MaxValue; // Garantiza que sea positivo
        } while (numero < minimo || numero >= maximo);

        return numero.ToString();
    }

}