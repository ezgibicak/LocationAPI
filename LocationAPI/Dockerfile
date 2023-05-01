#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LocationAPI/LocationAPI.csproj", "LocationAPI/"]
COPY ["Model/Model.csproj", "Model/"]
COPY ["Cache/Cache.csproj", "Cache/"]
COPY ["Business/Business.csproj", "Business/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Core/Core.csproj", "Core/"]
RUN dotnet restore "LocationAPI/LocationAPI.csproj"
COPY . .
WORKDIR "/src/LocationAPI"
RUN dotnet build "LocationAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LocationAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LocationAPI.dll"]