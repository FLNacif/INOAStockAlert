#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["INOA.Challenge.StockApi.Mock/INOA.Challenge.StockApi.Mock.csproj", "INOA.Challenge.StockApi.Mock/"]
RUN dotnet restore "INOA.Challenge.StockApi.Mock/INOA.Challenge.StockApi.Mock.csproj"
COPY . .
WORKDIR "/src/INOA.Challenge.StockApi.Mock"
RUN dotnet build "INOA.Challenge.StockApi.Mock.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "INOA.Challenge.StockApi.Mock.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "INOA.Challenge.StockApi.Mock.dll"]