# Executando com Docker Compose - Auth API

Este guia explica como executar o projeto Auth API usando Docker Compose, ideal para simular um ambiente de produção e testes integrados.

### 1. Configurar User Secrets

> **Importante:** No Windows, as secrets do Docker estão configuradas para usar o caminho do Windows (`${APPDATA}/Microsoft/UserSecrets`). No Linux/WSL, use o caminho padrão do Linux.

Entre no diretório do projeto WebApi:

```bash
cd src/AuthApi.WebApi
```

Execute os comandos para configurar os secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "JwtPrivateKey" "fedaf7d8863b48e197b9287d492b708e"
dotnet user-secrets set "AuthDbConnectionString" "Server=auth_mysql;Port=3306;Database=auth_db;Uid=auth_user;Pwd=root123;"
```

> **Nota:** A connection string usa `auth_mysql` como host, que é o nome do serviço no docker-compose.

**Configuração do Honeycomb (opcional):**

```bash
dotnet user-secrets set "OTEL_SERVICE_NAME" "auth-api"
dotnet user-secrets set "OTEL_EXPORTER_OTLP_PROTOCOL" "http/protobuf"
dotnet user-secrets set "OTEL_EXPORTER_OTLP_ENDPOINT" "https://api.honeycomb.io"
dotnet user-secrets set "OTEL_EXPORTER_OTLP_HEADERS" "x-honeycomb-team=SeuTokenDoHoneyComb"
```

> **Nota:** Para obter o token do Honeycomb, acesse [Honeycomb.io](https://www.honeycomb.io/), faça login e gere um API Key na seção de configurações.

### 2. Escolher o docker-compose correto

**Para Windows:**
- Use o arquivo `src/docker-compose_win.yml` que está configurado para usar as secrets do Windows (`${APPDATA}/Microsoft/UserSecrets`).

**Para Linux/WSL:**
- Use o arquivo `src/docker-compose.yml` que está configurado para usar as secrets do Linux (`~/.microsoft/usersecrets`).

### 3. Iniciar os serviços Docker

Volte para o diretório raiz:

```bash
cd ../..
```

**Windows:**
```bash
docker compose -f src/docker-compose_win.yml up -d --force-recreate
```

**Linux/WSL:**
```bash
docker compose -f src/docker-compose.yml up -d --force-recreate
```

Verifique se todos os containers foram iniciados:

```bash
docker ps
```

Você deve ver dois containers rodando:
- `auth_api` - Aplicação ASP.NET Core
- `auth_mysql` - Banco de dados MySQL

### 4. Executar migrações do banco de dados

Entre no projeto de infraestrutura e execute as migrações:

```bash
cd src/AuthApi.Infraestructure
dotnet ef database update
```

> **Nota:** As migrações são executadas localmente, mas se conectam ao MySQL rodando no Docker através da connection string configurada.

### 5. Acessar a aplicação

A API estará disponível em `http://localhost:5300/swagger`.

## 📝 Notas importantes

- No Windows, as secrets devem estar no caminho `${APPDATA}/Microsoft/UserSecrets`
- A connection string deve usar `auth_mysql` como host (nome do serviço no docker-compose)
- As migrações são executadas localmente, mas se conectam ao MySQL no Docker
