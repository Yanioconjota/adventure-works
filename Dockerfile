FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Copia el archivo de solución y todos los proyectos
COPY ["adventureworksapi/AdventureWorksApi.sln", "./"]
COPY ["adventureworksapi/Presentation/", "Presentation/"]
COPY ["adventureworksapi/Application/", "Application/"]
COPY ["adventureworksapi/Domain/", "Domain/"]
COPY ["adventureworksapi/Infrastructure/", "Infrastructure/"]

# Restaurar dependencias usando el archivo .sln
RUN dotnet restore

WORKDIR "/src/Presentation"
RUN dotnet build "Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.dll"]
