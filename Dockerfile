FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Copia el archivo de solución
COPY ["adventureworksapi/AdventureWorksApi.sln", "./"]
# Copia todos los archivos de proyecto csproj
COPY ["adventureworksapi/Presentation/Presentation.csproj", "Presentation/"]
COPY ["adventureworksapi/Application/Application.csproj", "Application/"]
COPY ["adventureworksapi/Domain/Domain.csproj", "Domain/"]
COPY ["adventureworksapi/Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Restaura las dependencias y herramientas de todos los proyectos
RUN dotnet restore "Presentation/Presentation.csproj"
RUN dotnet restore "Application/Application.csproj"
RUN dotnet restore "Domain/Domain.csproj"
RUN dotnet restore "Infrastructure/Infrastructure.csproj"

COPY ["adventureworksapi/Presentation/", "Presentation/"]
COPY ["adventureworksapi/Application/", "Application/"]
COPY ["adventureworksapi/Domain/", "Domain/"]
COPY ["adventureworksapi/Infrastructure/", "Infrastructure/"]

WORKDIR "/src/Presentation"
RUN dotnet build "Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.dll"]
