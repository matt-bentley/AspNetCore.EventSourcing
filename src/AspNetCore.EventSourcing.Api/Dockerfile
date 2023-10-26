FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim AS base

WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080;

FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS build

COPY ["src/", "/src/"]

WORKDIR /src/AspNetCore.EventSourcing.Api
RUN dotnet restore "AspNetCore.EventSourcing.Api.csproj" && \
    dotnet publish "AspNetCore.EventSourcing.Api.csproj" --no-restore -c Release -o /app/publish

FROM base AS final
COPY --from=build /app/publish .
USER 1000
ENTRYPOINT ["dotnet", "AspNetCore.EventSourcing.Api.dll"]