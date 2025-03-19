FROM mcr.microsoft.com/dotnet/sdk:8.0.101 AS build

WORKDIR /app
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["Auth_Api.WebApi/Auth_Api.WebApi.csproj", "Auth_Api.WebApi/"]
RUN dotnet restore "./Auth_Api.WebApi/Auth_Api.WebApi.csproj"

COPY . .
WORKDIR "/src/Auth_Api.WebApi"
RUN dotnet build "./Auth_Api.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Auth_Api.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0.1

WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Auth_Api.WebApi.dll"]