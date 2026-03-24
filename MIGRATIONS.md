# EF Core Migrations — FSI CloudShopping

## Pré-requisitos

1. .NET 9 SDK instalado
2. MySQL 8.0+ rodando localmente (ou via Docker)
3. `dotnet-ef` tool instalado globalmente:
   ```bash
   dotnet tool install --global dotnet-ef
   ```
4. String de conexão configurada em `src/FSI.CloudShopping.WebAPI/appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Port=3306;Database=cloudshopping;User=root;Password=SuaSenha;"
     }
   }
   ```

---

## Criar Migration Inicial

```bash
cd src/FSI.CloudShopping.WebAPI

dotnet ef migrations add InitialCreate \
  --project ../FSI.CloudShopping.Infrastructure \
  --startup-project . \
  --output-dir Data/Migrations
```

---

## Aplicar Migrations ao Banco

```bash
cd src/FSI.CloudShopping.WebAPI

dotnet ef database update \
  --project ../FSI.CloudShopping.Infrastructure \
  --startup-project .
```

> O `DatabaseSeeder` é executado automaticamente ao iniciar a aplicação em modo `Development`,
> populando categorias, produtos e cupons de exemplo.

---

## Reverter Migration

```bash
# Reverter para uma migration específica
dotnet ef database update NomeDaMigrationAnterior \
  --project ../FSI.CloudShopping.Infrastructure \
  --startup-project .

# Reverter tudo (banco vazio)
dotnet ef database update 0 \
  --project ../FSI.CloudShopping.Infrastructure \
  --startup-project .
```

---

## Adicionar Nova Migration (após mudanças no modelo)

```bash
cd src/FSI.CloudShopping.WebAPI

dotnet ef migrations add NomeDaMudanca \
  --project ../FSI.CloudShopping.Infrastructure \
  --startup-project . \
  --output-dir Data/Migrations
```

---

## Listar Migrations Aplicadas

```bash
dotnet ef migrations list \
  --project ../FSI.CloudShopping.Infrastructure \
  --startup-project src/FSI.CloudShopping.WebAPI
```

---

## Rodar via Docker (alternativa)

```bash
# Subir MySQL via Docker
docker run -d \
  --name cloudshopping-mysql \
  -e MYSQL_ROOT_PASSWORD=root123 \
  -e MYSQL_DATABASE=cloudshopping \
  -p 3306:3306 \
  mysql:8.0

# Aguardar MySQL inicializar (~15s), então:
cd src/FSI.CloudShopping.WebAPI
dotnet ef database update \
  --project ../FSI.CloudShopping.Infrastructure \
  --startup-project .
```

---

## Iniciar a Aplicação

```bash
cd src/FSI.CloudShopping.WebAPI
dotnet run

# Swagger UI disponível em:
# https://localhost:5001/swagger
# http://localhost:5000/swagger
```

---

## Variáveis de Ambiente Necessárias (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  },
  "JwtSettings": {
    "SecretKey": "sua-chave-secreta-de-pelo-menos-32-caracteres",
    "Issuer": "FSI.CloudShopping",
    "Audience": "CloudShoppingClients",
    "AccessTokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 30
  },
  "MercadoPago": {
    "AccessToken": "TEST-xxxx",
    "WebhookSecret": "sua-chave-webhook"
  },
  "SmtpSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Username": "seu-email@gmail.com",
    "Password": "sua-senha-de-app"
  }
}
```
