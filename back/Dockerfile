# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AA1.csproj", "./"]
RUN dotnet restore "./AA1.csproj"
COPY . .
RUN dotnet build "AA1.csproj" -c Release -o /app/build

# Etapa de publicación
FROM build AS publish
RUN dotnet publish "AA1.csproj" -c Release -o /app/publish

# Etapa final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .

# Obligamos a la aplicación a escuchar en el puerto 3000 en todas las interfaces de red
ENV ASPNETCORE_URLS=http://+:3000
# Informamos a Docker que el contenedor usa el puerto 3000
EXPOSE 3000

ENTRYPOINT ["dotnet", "AA1.dll"]