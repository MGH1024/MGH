FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MGH.EF.Review/MGH.EF.Review.csproj", "MGH.EF.Review/"]
RUN dotnet restore "MGH.EF.Review/MGH.EF.Review.csproj"
COPY . .
WORKDIR "/src/MGH.EF.Review"
RUN dotnet build "MGH.EF.Review.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MGH.EF.Review.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MGH.EF.Review.dll"]
