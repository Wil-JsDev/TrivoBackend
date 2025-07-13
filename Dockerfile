# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY Trivo.sln ./
COPY Trivo.Dominio/Trivo.Dominio.csproj ./Trivo.Dominio/
COPY Trivo.Aplicacion/Trivo.Aplicacion.csproj ./Trivo.Aplicacion/
COPY Trivo.Infraestructura.Compartido/Trivo.Infraestructura.Compartido.csproj ./Trivo.Infraestructura.Compartido/
COPY Trivo.Infraestructura.Persistencia/Trivo.Infraestructura.Persistencia.csproj ./Trivo.Infraestructura.Persistencia/
COPY Trivo.Presentacion.API/Trivo.Presentacion.API.csproj ./Trivo.Presentacion.API/

RUN dotnet restore

# Copiar el resto del contenido
COPY . .

# Establecer el directorio donde sí hay un .csproj
WORKDIR /app/Trivo.Presentacion.API

# Publicar el proyecto
RUN dotnet publish Trivo.Presentacion.API.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5026
ENTRYPOINT ["dotnet", "Trivo.Presentacion.API.dll"]
