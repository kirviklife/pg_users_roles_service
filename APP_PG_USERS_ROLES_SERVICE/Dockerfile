#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["APP_PG_USERS_ROLES_SERVICE/APP_PG_USERS_ROLES_SERVICE.csproj", "APP_PG_USERS_ROLES_SERVICE/"]
RUN dotnet restore "APP_PG_USERS_ROLES_SERVICE/APP_PG_USERS_ROLES_SERVICE.csproj"
COPY . .
WORKDIR "/src/APP_PG_USERS_ROLES_SERVICE"
RUN dotnet build "APP_PG_USERS_ROLES_SERVICE.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "APP_PG_USERS_ROLES_SERVICE.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APP_PG_USERS_ROLES_SERVICE.dll"]