#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CP.Api.csproj", "."]
RUN dotnet restore "./CP.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "CP.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CP.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet CP.Api.dll