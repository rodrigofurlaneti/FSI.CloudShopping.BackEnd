# =========================
# ETAPA 1 - BUILD
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia toda a solução
COPY . .

# Restaura pacotes a partir da solução
RUN dotnet restore src/FSI.CloudShopping.sln

# Publica APENAS a WebAPI
RUN dotnet publish src/FSI.CloudShopping.WebAPI/FSI.CloudShopping.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# =========================
# ETAPA 2 - RUNTIME
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copia somente o que foi publicado
COPY --from=build /app/publish .

# Porta que o Render espera
EXPOSE 8080

# Garante que o Kestrel escute na porta certa
ENV ASPNETCORE_URLS=http://+:8080

# Inicia a API
ENTRYPOINT ["dotnet", "FSI.CloudShopping.API.dll"]
