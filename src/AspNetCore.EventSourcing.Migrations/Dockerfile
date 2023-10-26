FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim AS base

WORKDIR /app

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS build

COPY ["src/", "/src/"]

WORKDIR /src/AspNetCore.EventSourcing.Migrations
RUN dotnet restore "AspNetCore.EventSourcing.Migrations.csproj" && \
    dotnet publish "AspNetCore.EventSourcing.Migrations.csproj" --no-restore -c Release -o /app/publish

FROM base AS final
COPY --from=build /app/publish .
USER 1000
ENTRYPOINT ["dotnet", "AspNetCore.EventSourcing.Migrations.dll"]