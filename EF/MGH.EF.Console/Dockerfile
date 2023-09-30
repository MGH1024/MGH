FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EF/MGH.EF.Console/MGH.EF.Console.csproj", "EF/MGH.EF.Console/"]
RUN dotnet restore "EF/MGH.EF.Console/MGH.EF.Console.csproj"
COPY . .
WORKDIR "/src/EF/MGH.EF.Console"
RUN dotnet build "MGH.EF.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MGH.EF.Console.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MGH.EF.Console.dll"]
