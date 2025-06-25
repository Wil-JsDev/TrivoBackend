using Trivo.Dominio.Enum;

namespace Trivo.Aplicacion.Utilidades;

/// <summary>
/// Representa un error ocurrido durante una operación, incluyendo un código, descripción y tipo.
/// </summary>
public class Error
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Error"/> con el código, descripción y tipo de error especificados.
    /// </summary>
    /// <param name="codigo">Un identificador único para el error.</param>
    /// <param name="descripcion">Una descripción legible para humanos del error.</param>
    /// <param name="tipoError">El tipo o categoría del error.</param>
    private Error(
        string codigo, 
        string descripcion, 
        ErrorTipo tipoError)
    {
        Codigo = codigo;
        Descripcion = descripcion;
        TipoError = tipoError;
    }

    /// <summary>
    /// Obtiene el código único que identifica el error.
    /// </summary>
    public string Codigo { get; }

    /// <summary>
    /// Obtiene la descripción del error.
    /// </summary>
    public string Descripcion { get; }

    /// <summary>
    /// Obtiene el tipo o categoría del error.
    /// </summary>
    public ErrorTipo TipoError { get; }

    /// <summary>
    /// Crea un error genérico de fallo.
    /// </summary>
    /// <param name="codigo">El código que identifica el error.</param>
    /// <param name="descripcion">Una descripción del fallo.</param>
    /// <returns>Una instancia de <see cref="Error"/> que representa un fallo.</returns>
    public static Error Fallo(string codigo, string descripcion) => 
        new Error(codigo, descripcion, ErrorTipo.Fallo);

    /// <summary>
    /// Crea un error de recurso no encontrado.
    /// </summary>
    /// <param name="codigo">El código que identifica el error.</param>
    /// <param name="descripcion">Una descripción que indica qué no se encontró.</param>
    /// <returns>Una instancia de <see cref="Error"/> que representa un error de no encontrado.</returns>
    public static Error NoEncontrado(string codigo, string descripcion) =>
        new Error(codigo, descripcion, ErrorTipo.NoEncontrado);
    
    /// <summary>
    /// Crea un error de conflicto, típicamente usado cuando un recurso ya existe o hay conflicto de datos.
    /// </summary>
    /// <param name="codigo">El código que identifica el error de conflicto.</param>
    /// <param name="descripcion">Una descripción del conflicto.</param>
    /// <returns>Una instancia de <see cref="Error"/> que representa un conflicto.</returns>
    public static Error Conflicto(string codigo, string descripcion) =>
        new Error(codigo, descripcion, ErrorTipo.Conflicto);
}
