using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Trivo.Aplicacion.Interfaces.Servicios.IA;

namespace Trivo.Infraestructura.Compartido.Servicios;

public class OllamaServicio : IOllamaServicio
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OllamaServicio> _logger;
    public OllamaServicio(ILogger<OllamaServicio> logger,HttpClient httpClient)
    {
        _httpClient = httpClient;
        _logger = logger;
        _logger.LogInformation("BaseAddress de HttpClient para OllamaServicio: {BaseAddress}", _httpClient.BaseAddress);
    }
    
    public async Task<string?> EnviarPeticionIaAsync(string modelo, string prompt)
    {
        var solicitud = new
        {
            model = modelo,
            stream = false,
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var respuesta = await _httpClient.PostAsJsonAsync(new Uri("http://localhost:4000/api/chat"), solicitud);
        respuesta.EnsureSuccessStatusCode();

        var contenidoJson = await respuesta.Content.ReadFromJsonAsync<RespuestaOllama>();
        return contenidoJson?.Message?.Content;
    }
}

public class RespuestaOllama
{
    public Message? Message { get; set; }
}

public class Message
{
    public string? Role { get; set; }
    public string? Content { get; set; }
}