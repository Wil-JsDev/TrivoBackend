namespace Trivo.Aplicacion.Utilidades;

public static class PaginacionExtension
{
    public static IEnumerable<T> Paginar<T>(this IEnumerable<T> fuente, int pagina, int tamanoPagina)
    {
        return fuente.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina);
    }
}