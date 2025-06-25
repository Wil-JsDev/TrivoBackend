using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Trivo.Aplicacion.Utilidades;

public static class ExtensionesCacheDistribuido
{
    private static DistributedCacheEntryOptions ExpiracionPorDefecto => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
    };

    public static async Task<T> ObtenerOCrearAsync<T>(
        this IDistributedCache cache,
        string clave,
        Func<Task<T>> fabrica,
        DistributedCacheEntryOptions opcionesCache = null!,
        CancellationToken cancellationToken = default
    )
    {
        var datosCacheados = await cache.GetStringAsync(clave, cancellationToken);

        if (datosCacheados != null)
        {
            Console.WriteLine($" Cache HIT para la clave: {clave}");
            return JsonSerializer.Deserialize<T>(datosCacheados)!;
        }

        Console.WriteLine($" Cache MISS para la clave: {clave}");

        var datos = await fabrica();

        await cache.SetStringAsync(
            clave,
            JsonSerializer.Serialize(datos),
            opcionesCache ?? ExpiracionPorDefecto,
            cancellationToken
        );

        Console.WriteLine($" Datos cacheados bajo la clave: {clave}");

        return datos;
    }
}
