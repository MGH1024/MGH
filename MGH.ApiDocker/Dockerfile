FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src/MGH.ApiDocker
COPY ["MGH.ApiDocker.csproj", ""]
RUN dotnet restore "MGH.ApiDocker.csproj"
COPY . .

RUN dotnet build "MGH.ApiDocker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MGH.ApiDocker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MGH.ApiDocker.dll"]
