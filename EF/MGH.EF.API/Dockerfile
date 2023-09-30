FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EF/MGH.EF.API/MGH.EF.API.csproj", "EF/MGH.EF.API/"]
RUN dotnet restore "EF/MGH.EF.API/MGH.EF.API.csproj"
COPY . .
WORKDIR "/src/EF/MGH.EF.API"
RUN dotnet build "MGH.EF.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MGH.EF.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MGH.EF.API.dll"]
