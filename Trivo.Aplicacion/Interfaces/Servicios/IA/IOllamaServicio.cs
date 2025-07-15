namespace Trivo.Aplicacion.Interfaces.Servicios.IA;

public interface IOllamaServicio
{
    Task<string?> EnviarPeticionIaAsync(string modelo, string prompt);
}