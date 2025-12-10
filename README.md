# Getting Started - Auth API

Este guia irá ajudá-lo a configurar e executar o projeto Auth API localmente ou com Docker.

## 📋 Pré-requisitos

Antes de começar, certifique-se de ter as seguintes ferramentas instaladas:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker Desktop](https://docs.docker.com/desktop/features/wsl/) (para execução com Docker)
- [Git](https://git-scm.com/downloads)

## 📥 Clonando o repositório

Após instalar todos os pré-requisitos, clone o repositório:

```bash
git clone https://github.com/diegoferreirax/auth-api.git
cd auth-api
```

## 🚀 Formas de execução

O projeto Auth API pode ser executado de duas formas:

### 1. Execução Local (sem Docker)

Ideal para desenvolvimento e depuração local.

### 2. Execução com Docker Compose

Ideal para simular um ambiente de produção e testes integrados.

Escolha uma das opções abaixo para continuar.

---

## 🖥️ Executando Localmente

Siga os passos abaixo para executar o projeto localmente:

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

As configurações também podem ser definidas no arquivo `appsettings.Development.json`:

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

---

## 🐳 Executando com Docker Compose

Siga os passos abaixo para executar o projeto com Docker:

### 1. Configurar User Secrets (Windows)

> **Importante:** No Windows, as secrets do Docker estão configuradas para usar o caminho do Windows (`${APPDATA}/Microsoft/UserSecrets`).

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

**Configuração do Honeycomb (opcional):**

```bash
dotnet user-secrets set "OTEL_SERVICE_NAME" "auth-api"
dotnet user-secrets set "OTEL_EXPORTER_OTLP_PROTOCOL" "http/protobuf"
dotnet user-secrets set "OTEL_EXPORTER_OTLP_ENDPOINT" "https://api.honeycomb.io"
dotnet user-secrets set "OTEL_EXPORTER_OTLP_HEADERS" "x-honeycomb-team=SeuTokenDoHoneyComb"
```

### 2. Escolher o docker-compose correto

**Para Windows:**
- Use o arquivo `src/docker-compose_win.yml` que está configurado para usar as secrets do Windows.

**Para Linux/WSL:**
- Use o arquivo `src/docker-compose.yml` que está configurado para usar as secrets do Linux.

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

### 4. Executar migrações do banco de dados

Entre no projeto de infraestrutura e execute as migrações:

```bash
cd src/AuthApi.Infraestructure
dotnet ef database update
```

### 5. Acessar a aplicação

A API estará disponível em `http://localhost:5300/swagger`.

---

## ⚙️ Variáveis de Ambiente

O projeto suporta configuração através de variáveis de ambiente. As seguintes variáveis podem ser definidas:

| Variável                  | Descrição                                    | Exemplo (Local)                                    | Exemplo (Docker)                                    |
| ------------------------- | -------------------------------------------- | -------------------------------------------------- | --------------------------------------------------- |
| `AuthDbConnectionString`  | String de conexão com o banco de dados MySQL | `Server=localhost;Port=3306;Database=auth_db;Uid=auth_user;Pwd=root123;` | `Server=auth_mysql;Port=3306;Database=auth_db;Uid=auth_user;Pwd=root123;` |
| `JwtPrivateKey`           | Chave privada para geração de tokens JWT     | `fedaf7d8863b48e197b9287d492b708e`                | `fedaf7d8863b48e197b9287d492b708e`                  |
| `OTEL_SERVICE_NAME`       | Nome do serviço no OpenTelemetry            | `auth-api`                                         | `auth-api`                                          |
| `OTEL_EXPORTER_OTLP_PROTOCOL` | Protocolo do exportador OTLP            | `http/protobuf`                                    | `http/protobuf`                                     |
| `OTEL_EXPORTER_OTLP_ENDPOINT` | Endpoint do exportador OTLP             | `https://api.honeycomb.io`                         | `https://api.honeycomb.io`                          |
| `OTEL_EXPORTER_OTLP_HEADERS` | Headers do exportador OTLP              | `x-honeycomb-team=SeuTokenDoHoneyComb`             | `x-honeycomb-team=SeuTokenDoHoneyComb`              |

---

## 🧪 Testando a aplicação

Após iniciar a aplicação, você pode testá-la das seguintes formas:

### 1. Acessar o Swagger

Abra o navegador e acesse:

- **Local:** `http://localhost:5300/swagger`
- **Docker:** `http://localhost:5300/swagger`

### 2. Usar a collection do Postman

A collection do Postman está localizada na pasta `.doc/postman`. Ela contém todos os requests e payloads necessários para testar os endpoints da API.

### 3. Usar o JMeter

O projeto inclui um plano de testes configurado no JMeter para testes de carga e performance.

Para instruções detalhadas sobre como configurar e executar os testes com JMeter, consulte o [README do JMeter](.doc/jmeter/README.md).

---

## 🚀 Features

- Registros de usuário (create)
- Listagem de usuários com paginação (read)
- Exclusão de usuário (delete)
- Autenticação de usuário
- Criptografia de senha com BCrypt.Net
- Controle de acesso por função (Role-Based Access Control - RBAC) para alguns endpoints
- Utilização de Resource.resx para centralizar textos e mensagens em geral
- Uso do EF Core para facilitar e escalar a criação da estrutura da base de dados
- Monitoramento de logs com OpenTelemetry e Honeycomb

## 🔮 Features Futuras

- Publicação com Azure DevOps e Azure App Services
- Utilização do banco MySql na Oracle
- Envio de código de confirmação de email

## 🧩 Patterns

- Arquitetura de `Vertical Slice` com Command, Handler e Endpoint separados para cada feature do dominio
- Versionamento das pastas e end-points de features para possíveis atualizações de escopo maior sem mudar a versão existente
- Projeto `AuthApi.Infraestructure` exclusivo para migrações do EF Core para separar a infraestrutura da base da regra de negócio
- Utilização da struct `Maybe` para tratamento de nullos
- Uso de `record` para objetos de request e response
- Criação de `UnitOfWork` no EF Core para centralizar as operações de mudanças
- Utilização de `primary constructor` nas classes que dependem de injeção de dependência

---

## 🔍 Troubleshooting

### Problemas comuns

**Erro ao iniciar o container:**
- Verifique se o Docker está rodando
- Verifique se as portas 5300 e 3306 não estão em uso
- Execute `docker compose down` antes de tentar novamente

**Erro de conexão com o banco:**
- Verifique se o container `auth_mysql` está rodando: `docker ps`
- Verifique se a connection string está correta
- Aguarde alguns segundos após iniciar o MySQL para garantir que está totalmente inicializado

**Erro com User Secrets no Windows:**
- Certifique-se de estar usando o arquivo `docker-compose_win.yml`
- Verifique se as secrets foram configuradas corretamente com `dotnet user-secrets list`

**Erro ao executar migrações:**
- Verifique se o banco de dados está acessível
- Verifique se a connection string está correta
- Certifique-se de estar no diretório correto (`src/AuthApi.Infraestructure`)
