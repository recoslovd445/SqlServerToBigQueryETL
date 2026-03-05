FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
# Esta línea es VITAL para que Google Cloud Run encuentre tu app
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiamos solo el proyecto para restaurar dependencias
# (Como el Dockerfile está al lado del .csproj, la ruta es directa)
COPY ["SqlServerToBigQueryETL.csproj", "./"]
RUN dotnet restore "SqlServerToBigQueryETL.csproj"

# Copiamos todo lo demás y compilamos
COPY . .
RUN dotnet build "SqlServerToBigQueryETL.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SqlServerToBigQueryETL.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SqlServerToBigQueryETL.dll"]
