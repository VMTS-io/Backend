# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8081
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/VMTS.API/VMTS.API.csproj", "./VMTS.API/"]
COPY ["src/VMTS.Service/VMTS.Service.csproj", "./VMTS.Service/"]
COPY ["src/VMTS.Repository/VMTS.Repository.csproj", "./VMTS.Repository/"]
COPY ["src/VMTS.Core/VMTS.Core.csproj", "./VMTS.Core/"]
RUN dotnet restore "./VMTS.API/VMTS.API.csproj"
WORKDIR /src/VMTS.API/
COPY . .
RUN dotnet build "./VMTS.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VMTS.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
# Set the ASP.NET Core URLs to listen on both HTTP and HTTPS
# ENV ASPNETCORE_URLS="https://+:8081;http://+:8080"
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VMTS.API.dll"]
