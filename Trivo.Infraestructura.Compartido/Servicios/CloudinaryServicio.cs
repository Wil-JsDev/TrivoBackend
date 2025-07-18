using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Trivo.Aplicacion.Interfaces.Servicios;
using Trivo.Dominio.Configuraciones;

namespace Trivo.Infraestructura.Compartido.Servicios;

public sealed class CloudinaryServicio : ICloudinaryServicio
{
    private CloudinaryConfiguraciones _cloudinary { get; }

    public CloudinaryServicio(IOptions<CloudinaryConfiguraciones> cloudinary)
    {
        _cloudinary = cloudinary.Value;
    }
    
    public async Task<string> SubirImagenAsync(
        Stream archivo,
        string nombreImagen,
        CancellationToken cancellationToken)
    {
        Cloudinary cloudinary = new (_cloudinary.CloudinaryUrl);
        ImageUploadParams image = new()
        {
            File = new FileDescription(nombreImagen, archivo),
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = true
        };
            
        var subirResultado = await cloudinary.UploadAsync(image,cancellationToken);
        
        return subirResultado.SecureUrl.ToString();
    }

    public async Task<string> SubirArchivoAsync(Stream archivo, string nombreArchivo, CancellationToken cancellationToken)
    {
        {
            var cloudinary = new Cloudinary(_cloudinary.CloudinaryUrl);
            var subirArchivo = new RawUploadParams
            {
                File = new FileDescription(nombreArchivo, archivo),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };

  RawUploadResult subirResultado = await cloudinary.UploadAsync(subirArchivo, "raw", cancellationToken);
            return subirResultado.SecureUrl.ToString();
        }
    }
}

