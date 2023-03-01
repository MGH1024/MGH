FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MGH.ConsoleDocker/MGH.ConsoleDocker.csproj", "MGH.ConsoleDocker/"]
RUN dotnet restore "MGH.ConsoleDocker/MGH.ConsoleDocker.csproj"
COPY . .
WORKDIR "/src/MGH.ConsoleDocker"
RUN dotnet build "MGH.ConsoleDocker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MGH.ConsoleDocker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MGH.ConsoleDocker.dll"]
