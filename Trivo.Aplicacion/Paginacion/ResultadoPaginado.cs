namespace Trivo.Aplicacion.Paginacion;

public class ResultadoPaginado<T>
{
    public ResultadoPaginado()
    {
        
    }

    public ResultadoPaginado(IEnumerable<T>? elementos, int totalElementos, int paginaActual, int tamanioPagina)
    {
        Elementos = elementos;
        TotalElementos = totalElementos;
        PaginaActual = paginaActual;
        TotalPaginas = (int)Math.Ceiling(totalElementos / (double)tamanioPagina);
    }
    
    public IEnumerable<T>? Elementos { get; set; }
    public int TotalElementos { get; set; }
    public int PaginaActual { get; set; }
    public int TotalPaginas { get; set; }
}