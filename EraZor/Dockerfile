# Brug den officielle ASP.NET Core-runtime som base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002
EXPOSE 5003

# Brug SDK-billedet til at bygge applikationen
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopiér projektfil og udfør restore
COPY ["EraZor.csproj", "./"]
RUN dotnet restore "EraZor.csproj"

# Kopiér resten af applikationen
COPY . .
RUN dotnet build -c Release -o /app/build

# Publicer applikationen
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Brug runtime-billedet til at køre applikationen
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "EraZor.dll"]
