FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/APIs/Ailos.ContaCorrente.API/Ailos.ContaCorrente.API.csproj", "src/APIs/Ailos.ContaCorrente.API/"]
COPY ["src/Application/Ailos.ContaCorrente.Application/Ailos.ContaCorrente.Application.csproj", "src/Application/Ailos.ContaCorrente.Application/"]
COPY ["src/Domain/Ailos.ContaCorrente.Domain/Ailos.ContaCorrente.Domain.csproj", "src/Domain/Ailos.ContaCorrente.Domain/"]
COPY ["src/Infrastructure/Ailos.ContaCorrente.Infrastructure/Ailos.ContaCorrente.Infrastructure.csproj", "src/Infrastructure/Ailos.ContaCorrente.Infrastructure/"]
COPY ["src/Infrastructure/Ailos.Shared.Infrastructure/Ailos.Shared.Infrastructure.csproj", "src/Infrastructure/Ailos.Shared.Infrastructure/"]
COPY ["src/Shared/Ailos.Shared.Common/Ailos.Shared.Common.csproj", "src/Shared/Ailos.Shared.Common/"]
RUN dotnet restore "src/APIs/Ailos.ContaCorrente.API/Ailos.ContaCorrente.API.csproj"
COPY . .
WORKDIR "/src/src/APIs/Ailos.ContaCorrente.API"
RUN dotnet build "Ailos.ContaCorrente.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ailos.ContaCorrente.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ailos.ContaCorrente.API.dll"]
