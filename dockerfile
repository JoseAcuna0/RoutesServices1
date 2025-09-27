# --------------------------
# Etapa 1: Build
# --------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar todo el código y compilar
COPY . .
RUN dotnet publish -c Release -o /app

# --------------------------
# Etapa 2: Runtime
# --------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiar el resultado del build
COPY --from=build /app .

# Configuración del puerto (Render usa el puerto 10000 internamente)
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Ejecutar el servicio
ENTRYPOINT ["dotnet", "RoutesService.dll"]
