using System.Text.Json.Serialization;

namespace Trivo.Aplicacion.Utilidades;

 /// <summary>
    /// Representa el resultado de una operación que puede ser exitosa o fallida.
    /// </summary>
    public class Resultado
    {
        /// <summary>
        /// Obtiene un valor que indica si la operación fue exitosa.
        /// </summary>
        public bool EsExitoso { get; set; }

        /// <summary>
        /// Obtiene la información del error si la operación falló.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Error? Error { get; set; }

        /// <summary>
        /// Inicializa un resultado exitoso.
        /// </summary>
        protected Resultado()
        {
            EsExitoso = true;
            Error = default;
        }

        /// <summary>
        /// Inicializa un resultado fallido con el error especificado.
        /// </summary>
        /// <param name="error">El error que describe la falla.</param>
        protected Resultado(Error error)
        {
            EsExitoso = false;
            Error = error;
        }

        /// <summary>
        /// Convierte implícitamente un <see cref="Error"/> a un <see cref="Resultado"/> fallido.
        /// </summary>
        /// <param name="error">El error a convertir.</param>
        public static implicit operator Resultado(Error error) =>
            new(error);

        /// <summary>
        /// Crea un resultado exitoso.
        /// </summary>
        /// <returns>Un nuevo <see cref="Resultado"/> que indica éxito.</returns>
        public static Resultado Exito() =>
            new();

        /// <summary>
        /// Crea un resultado fallido con el error especificado.
        /// </summary>
        /// <param name="error">El error que describe la falla.</param>
        /// <returns>Un nuevo <see cref="Resultado"/> que indica fallo.</returns>
        public static Resultado Fallo(Error error) =>
            new(error);
    }

    /// <summary>
    /// Representa el resultado de una operación que puede ser exitosa con un valor de tipo <typeparamref name="TValor"/>
    /// o fallida con un error asociado.
    /// </summary>
    /// <typeparam name="TValor">El tipo del valor contenido en el resultado cuando la operación es exitosa.</typeparam>
    public class ResultadoT<TValor> : Resultado
    {
        /// <summary>
        /// Contiene el valor del resultado en caso de éxito.
        /// </summary>
        private readonly TValor? _valor;

        /// <summary>
        /// Inicializa un resultado exitoso con el valor especificado.
        /// </summary>
        /// <param name="valor">El valor asociado al resultado exitoso.</param>
        private ResultadoT(TValor valor) : base()
        {
            _valor = valor;
        }

        /// <summary>
        /// Inicializa un resultado fallido con el error especificado.
        /// </summary>
        /// <param name="error">El error que describe la falla.</param>
        private ResultadoT(Error error) : base(error)
        {
            _valor = default;
        }

        /// <summary>
        /// Obtiene el valor del resultado si la operación fue exitosa; de lo contrario, lanza una excepción.
        /// </summary>
        /// <exception cref="InvalidOperationException">Se lanza si el resultado indica fallo.</exception>
        public TValor Valor =>
            EsExitoso ? _valor! : throw new InvalidOperationException("No se puede acceder al valor cuando EsExitoso es falso");

        /// <summary>
        /// Convierte implícitamente un <see cref="Error"/> a un <see cref="ResultadoT{TValor}"/> fallido.
        /// </summary>
        /// <param name="error">El error a convertir.</param>
        public static implicit operator ResultadoT<TValor>(Error error) =>
            new(error);

        /// <summary>
        /// Convierte implícitamente un valor de tipo <typeparamref name="TValor"/> a un <see cref="ResultadoT{TValor}"/> exitoso.
        /// </summary>
        /// <param name="valor">El valor a convertir.</param>
        public static implicit operator ResultadoT<TValor>(TValor valor) =>
            new(valor);

        /// <summary>
        /// Crea un resultado exitoso con el valor proporcionado.
        /// </summary>
        /// <param name="valor">El valor asociado al resultado exitoso.</param>
        /// <returns>Una instancia de <see cref="ResultadoT{TValor}"/> que representa éxito.</returns>
        public static ResultadoT<TValor> Exito(TValor valor) =>
            new(valor);

        /// <summary>
        /// Crea un resultado fallido con el error proporcionado.
        /// </summary>
        /// <param name="error">El error asociado al resultado fallido.</param>
        /// <returns>Una instancia de <see cref="ResultadoT{TValor}"/> que representa fallo.</returns>
        public static ResultadoT<TValor> Fallo(Error error) =>
            new(error);
    }