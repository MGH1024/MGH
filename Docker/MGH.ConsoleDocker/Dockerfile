FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR "/src/ConsoleDocker"
COPY ["MGH.ConsoleDocker.csproj", "."]
RUN dotnet restore "MGH.ConsoleDocker.csproj"
COPY . .

RUN dotnet build "MGH.ConsoleDocker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MGH.ConsoleDocker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MGH.ConsoleDocker.dll"]
