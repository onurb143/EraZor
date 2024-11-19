# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["EraZor.csproj", "./"]
RUN dotnet restore "./EraZor.csproj"

# Copy all files and set the working directory
COPY . .
WORKDIR "/src"

# Build the application
RUN dotnet build "EraZor.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "EraZor.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy only the published output to the runtime container
COPY --from=publish /app/publish .

# Expose necessary ports
EXPOSE 5000
EXPOSE 5001

# Set the entry point for the container
ENTRYPOINT ["dotnet", "EraZor.dll"]
