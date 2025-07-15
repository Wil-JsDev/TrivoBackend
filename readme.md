# Trivo - Backend

Este proyecto utiliza **ASP.NET Core** como backend y **PostgreSQL** como base de datos, todo orquestado mediante **Docker Compose** para facilitar el entorno de desarrollo.

---

## Levantar contenedores

Asegúrate de tener instalado:

- Docker Desktop

### Comando para iniciar el entorno

```bash
docker compose up -d
```

---

### Comando para iniciar entorno de produccion

```bash
docker compose -f docker-compose.prod.yml up -d
```
## Configuración del archivo .env


El proyecto requiere variables de entorno para funcionar correctamente. Estas se definen en un archivo .env que debe crearse a partir del archivo .env.template.

Pasos para crear tu archivo .env
Copia el archivo de plantilla:

```bash
cp .env.template .env
```
Abre el archivo .env copiado y completa los valores necesarios según tu entorno local.

Guarda los cambios. Docker Compose usará automáticamente este archivo al levantar los contenedores.