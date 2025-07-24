namespace Trivo.Aplicacion.Modulos.Administrador.Querys.ObtenerUsuariosBaneados;

public sealed record ObtenerUsuariosBaneadosConPaginacionQuery
(
    int NumeroPagina,
    int TamanoPagina
);