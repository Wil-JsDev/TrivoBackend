namespace Trivo.Aplicacion.Interfaces.Servicios;

public interface ICloudinaryServicio
{
    Task<string> SubirImagenAsync(Stream archivo, string nombreImagen, CancellationToken cancellationToken);
}