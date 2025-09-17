FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/APIs/Ailos.Transferencia.API/Ailos.Transferencia.API.csproj", "src/APIs/Ailos.Transferencia.API/"]
COPY ["src/Application/Ailos.Transferencia.Application/Ailos.Transferencia.Application.csproj", "src/Application/Ailos.Transferencia.Application/"]
COPY ["src/Domain/Ailos.Transferencia.Domain/Ailos.Transferencia.Domain.csproj", "src/Domain/Ailos.Transferencia.Domain/"]
COPY ["src/Infrastructure/Ailos.Transferencia.Infrastructure/Ailos.Transferencia.Infrastructure.csproj", "src/Infrastructure/Ailos.Transferencia.Infrastructure/"]
COPY ["src/Infrastructure/Ailos.Shared.Infrastructure/Ailos.Shared.Infrastructure.csproj", "src/Infrastructure/Ailos.Shared.Infrastructure/"]
COPY ["src/Shared/Ailos.Shared.Common/Ailos.Shared.Common.csproj", "src/Shared/Ailos.Shared.Common/"]
RUN dotnet restore "src/APIs/Ailos.Transferencia.API/Ailos.Transferencia.API.csproj"
COPY . .
WORKDIR "/src/src/APIs/Ailos.Transferencia.API"
RUN dotnet build "Ailos.Transferencia.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ailos.Transferencia.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ailos.Transferencia.API.dll"]
