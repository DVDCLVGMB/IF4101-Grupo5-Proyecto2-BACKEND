# ─── Stage 1: Build ─────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Steady-Management.WebAPI/Steady-Management.WebAPI.csproj", "Steady-Management.WebAPI/"]
RUN dotnet restore "Steady-Management.WebAPI/Steady-Management.WebAPI.csproj"

COPY . .
WORKDIR "/src/Steady-Management.WebAPI"
RUN dotnet publish -c Release -o /app/publish

# ─── Stage 2: Runtime ───────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "Steady-Management.WebAPI.dll"]
