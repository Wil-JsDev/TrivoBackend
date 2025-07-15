using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trivo.Aplicacion.Interfaces.Servicios.IA;
using Trivo.Dominio.Configuraciones;

namespace Trivo.Infraestructura.Compartido.Servicios;

public class OllamaServicio : IOllamaServicio
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OllamaServicio> _logger;
    private readonly OllamaOpciones _ollamaOpciones; 

    public OllamaServicio(
        ILogger<OllamaServicio> logger,
        HttpClient httpClient,
        IOptions<OllamaOpciones> ollamaOpciones)
    {
        _httpClient = httpClient;
        _logger = logger;
        _ollamaOpciones = ollamaOpciones.Value; 
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

        var url = $"{_ollamaOpciones.BaseUrl.TrimEnd('/')}/api/chat";
        var respuesta = await _httpClient.PostAsJsonAsync(url, solicitud);
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