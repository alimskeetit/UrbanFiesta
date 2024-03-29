FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["/UrbanFiesta/UrbanFiesta.csproj", "UrbanFiesta/"]
COPY ["/Entities/Entities.csproj", "Entities/"]
RUN dotnet restore "UrbanFiesta/UrbanFiesta.csproj"
COPY . .
WORKDIR "/src/UrbanFiesta"
RUN dotnet build "UrbanFiesta.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UrbanFiesta.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UrbanFiesta.dll"]