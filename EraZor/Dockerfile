# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Kopier csproj og gendan afhængigheder
COPY EraZor.csproj .
RUN dotnet restore EraZor.csproj

# Kopier resten af koden og byg applikationen
COPY . .
RUN dotnet publish -c Release -o /publish --property:GenerateStaticWebAssets=true

# Stage 2: Runtime (inkluderer værktøjer og SDK)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS runtime
WORKDIR /app

# Kopier publicerede filer fra build-stagen
COPY --from=build /publish .

# Kopier projektfilerne for EF-kommandosupport
COPY EraZor.csproj .

# Installer netværksværktøjer og dotnet-ef
RUN apt-get update && apt-get install -y iputils-ping && \
    dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Start applikationen
ENTRYPOINT ["dotnet", "EraZor.dll"]