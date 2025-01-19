# Base image with runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

# Build image with SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY JobScraper.API/JobScraperAPI.csproj JobScraper.API/
RUN dotnet restore "JobScraper.API/JobScraperAPI.csproj"

# Copy the rest of the source code and build
COPY JobScraper.API/ JobScraper.API/
WORKDIR /src/JobScraper.API
RUN dotnet build "JobScraperAPI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "JobScraperAPI.csproj" -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


ENTRYPOINT ["dotnet", "JobScraperAPI.dll"]
