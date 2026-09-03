FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first for layer caching
COPY Krinexa.sln .
COPY src/Krinexa.Api/Krinexa.Api.csproj src/Krinexa.Api/
COPY src/Krinexa.Application/Krinexa.Application.csproj src/Krinexa.Application/
COPY src/Krinexa.Domain/Krinexa.Domain.csproj src/Krinexa.Domain/
COPY src/Krinexa.Infrastructure/Krinexa.Infrastructure.csproj src/Krinexa.Infrastructure/

RUN dotnet restore

# Copy all source files and build
COPY . .
RUN dotnet publish src/Krinexa.Api/Krinexa.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render passes PORT as env var — must listen on it
ENV ASPNETCORE_URLS=http://+:${PORT}

ENTRYPOINT ["dotnet", "Krinexa.Api.dll"]
