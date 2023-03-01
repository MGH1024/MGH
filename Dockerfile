FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Console/Console.csproj", "Console/"]
RUN dotnet restore "Console/Console.csproj"
COPY . .
WORKDIR "/src/Console"
RUN dotnet build "Console.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/Console"

RUN dotnet publish "Console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Console.dll"]
