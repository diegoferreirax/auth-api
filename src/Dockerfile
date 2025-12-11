FROM mcr.microsoft.com/dotnet/sdk:8.0.101 AS build

WORKDIR /app
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["AuthApi.WebApi/AuthApi.WebApi.csproj", "AuthApi.WebApi/"]
RUN dotnet restore "./AuthApi.WebApi/AuthApi.WebApi.csproj"

COPY . .
RUN rm -rf ./AuthApi.WebApi/appsettings*

WORKDIR "/src/AuthApi.WebApi"
RUN dotnet build "./AuthApi.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuthApi.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0.1

WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "AuthApi.WebApi.dll"]