# ── Stage 1: Build ────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and restore (layer cache)
COPY Directory.Packages.props ./
COPY ClassHub.sln ./
COPY src/ClassHub.Web/ClassHub.Web.csproj             src/ClassHub.Web/
COPY tests/ClassHub.Tests/ClassHub.Tests.csproj       tests/ClassHub.Tests/
COPY tests/ClassHub.Specs/ClassHub.Specs.csproj       tests/ClassHub.Specs/
RUN dotnet restore

# Copy everything and build
COPY . .
RUN dotnet build -c Release --no-restore

# ── Stage 2: Test ─────────────────────────────────────────
FROM build AS test
RUN dotnet test -c Release --no-build --logger "console;verbosity=normal"

# ── Stage 3: Publish ──────────────────────────────────────
FROM build AS publish
RUN dotnet publish src/ClassHub.Web/ClassHub.Web.csproj \
    -c Release --no-build -o /app/publish

# ── Stage 4: Runtime ──────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create uploads directory
RUN mkdir -p uploads/recursos uploads/libros

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080
ENTRYPOINT ["dotnet", "ClassHub.Web.dll"]
