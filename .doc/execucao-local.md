# Executando Localmente - Auth API

Este guia explica como executar o projeto Auth API localmente sem Docker, ideal para desenvolvimento e depuração.

### 1. Configurar User Secrets

Entre no diretório do projeto WebApi:

```bash
cd src/AuthApi.WebApi
```

Execute os seguintes comandos para configurar os secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "JwtPrivateKey" "fedaf7d8863b48e197b9287d492b708e"
dotnet user-secrets set "AuthDbConnectionString" "Server=localhost;Port=3306;Database=auth_db;Uid=auth_user;Pwd=root123;"
```

**Configuração do Honeycomb (opcional):**

```bash
dotnet user-secrets set "OTEL_SERVICE_NAME" "auth-api"
dotnet user-secrets set "OTEL_EXPORTER_OTLP_PROTOCOL" "http/protobuf"
dotnet user-secrets set "OTEL_EXPORTER_OTLP_ENDPOINT" "https://api.honeycomb.io"
dotnet user-secrets set "OTEL_EXPORTER_OTLP_HEADERS" "x-honeycomb-team=SeuTokenDoHoneyComb"
```

> **Nota:** Para obter o token do Honeycomb, acesse [Honeycomb.io](https://www.honeycomb.io/), faça login e gere um API Key na seção de configurações.

### 2. Configurar variáveis no appsettings.Development.json

Alternativamente, as configurações podem ser definidas no arquivo `appsettings.Development.json`:

```json
{
  "AuthDbConnectionString": "Server=localhost;Port=3306;Database=auth_db;Uid=auth_user;Pwd=root123;",
  "JwtPrivateKey": "fedaf7d8863b48e197b9287d492b708e",
  "OTEL_SERVICE_NAME": "auth-api",
  "OTEL_EXPORTER_OTLP_PROTOCOL": "http/protobuf",
  "OTEL_EXPORTER_OTLP_ENDPOINT": "https://api.honeycomb.io",
  "OTEL_EXPORTER_OTLP_HEADERS": "x-honeycomb-team=SeuTokenDoHoneyComb"
}
```

### 3. Iniciar o banco de dados MySQL

Volte para o diretório raiz e inicie apenas o MySQL no Docker:

```bash
cd ../..
docker compose -f src/docker-compose.yml up -d auth_mysql
```

Aguarde alguns segundos para garantir que o MySQL esteja totalmente inicializado. Você pode verificar se está rodando com:

```bash
docker ps
```

### 4. Executar migrações do banco de dados

Entre no projeto de infraestrutura e execute as migrações:

```bash
cd src/AuthApi.Infraestructure
dotnet ef database update
```

### 5. Executar a aplicação

Volte para o projeto WebApi e execute:

```bash
cd ../AuthApi.WebApi
dotnet run
```

A API estará disponível em `http://localhost:5300/swagger`.

## 📝 Notas importantes

- A aplicação roda localmente, mas o MySQL ainda é executado via Docker para facilitar a configuração
- As configurações podem ser feitas via User Secrets (recomendado) ou via `appsettings.Development.json`

