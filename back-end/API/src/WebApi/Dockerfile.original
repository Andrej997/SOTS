#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["API/src/API.WebApi/API.WebApi.csproj", "API/src/API.WebApi/"]
RUN dotnet restore "API/src/API.WebApi/API.WebApi.csproj"
COPY . .
WORKDIR "/src/API/src/API.WebApi"
RUN dotnet build "API.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.WebApi.dll"]