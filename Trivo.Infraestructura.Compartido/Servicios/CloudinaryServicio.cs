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
    
    
}

