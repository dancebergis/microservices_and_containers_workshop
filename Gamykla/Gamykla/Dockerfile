# Configure the run time environment
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

# Configure the build process
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY . .

# Trigger the restore/build of the project
RUN dotnet build "Gamykla.csproj" -c Release -o /app

# With the build completed, publish the application
FROM build AS publish
RUN dotnet publish "Gamykla.csproj" -c Release -o /app

# Now execute the worker using dotnet runtimes
FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# Configure Kestrel Envrionment
ENV ASPNETCORE_URLS http://+:5999
EXPOSE 5999

# Execute program
ENTRYPOINT ["dotnet", "Gamykla.dll"]