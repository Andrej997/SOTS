#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["API/src/WebApi/WebApi.csproj", "API/src/WebApi/"]
COPY ["API/src/Application/Application.csproj", "API/src/Application/"]
COPY ["API/src/Domain/Domain.csproj", "API/src/Domain/"]
COPY ["API/src/Infrastructure/Infrastructure.csproj", "API/src/Infrastructure/"]
RUN dotnet restore "API/src/WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/API/src/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.WebApi.dll"]
