# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Kopier csproj og gendan afhængigheder
COPY EraZor.csproj .
RUN dotnet restore EraZor.csproj

# Kopier resten af koden og byg applikationen
COPY . .
RUN dotnet publish -c Release -o /publish --property:GenerateStaticWebAssets=true

# Stage 2: Migrations
FROM build AS migrations
WORKDIR /app
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Kør migrations
RUN dotnet ef database update

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "EraZor.dll"]
