# Auth API

Projeto de autenticação e registro de usuários para fins de portfólio e atualizações, utilizando .NET, EF Core, MySql e JWT como as principais tecnologias para o desenvolvimento. No momento não está publicada em nenhuma cloud, pois o github não contém fluxo de publicação com C#.     

## 🛠️ Tecnologias

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [EF Core](https://learn.microsoft.com/pt-br/ef/core/)
- [MySql](https://www.mysql.com/)
- [JWT](https://jwt.io/)
- [Honeycomb.io](https://www.honeycomb.io/)  

## 🚀 Features

- Registros de usuário (create)
- Listagem de usuários com paginação (read)
- Exclusão de usuário (delete)
- Autenticação de usuário
- Criptografia de senha com BCrypt.Net
- Controle de acesso por função (Role-Based Access Control - RBAC) para alguns endpoints
- Utilização de Resource.resx para centralizar textos e mensagens em geral
- Uso do EF Core para facilitar e escalar a criação da estrutura da base de dados e evitar SQL Injection
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

## ⚙️ Configuração

Para configurar o ambiente de desenvolvimento, siga os passos abaixo:

1. **Configurar o .NET 8**
   - Certifique-se de ter o [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) instalado.
   - Para verificar a instalação, execute:
     ```sh
     dotnet --version
     ```

2. **Baixar e instalar o Docker WSL (se for rodar com o docker)**
   - O projeto utiliza Docker no WSL (ou linux) para gerenciar os serviços necessários. Certifique-se de baixar e instalar o [Docker WSL](https://docs.docker.com/desktop/features/wsl/) ou utilizar o sistema operacional linux para rodar os serviços corretamente.

3. **Obter Api Key para visualizar logs no Honeycomb (etapa não obrigatória)**   
   - O projeto utiliza o [Honeycomb](https://www.honeycomb.io/) para monitoramento e análise de logs.  
   - Para obter o token de API, siga os passos abaixo:  
     1. Acesse [Honeycomb.io](https://www.honeycomb.io/) e crie uma conta (caso ainda não tenha).  
     2. Após fazer login, vá até a seção **API Keys** no painel de configurações.  
     3. Gere um novo **API Key** e copie o valor gerado.  
     4. Esse token será necessário para configurar os secrets da aplicação nas próximas etapas.  


## ▶️ Baixar e iniciar o projeto

1. **Baixar o projeto**
   - Clone o repositório utilizando o comando:
     ```sh
     git clone https://github.com/diegoferreirax/auth-api.git
     ```
   - Alternativamente, faça o download do código-fonte manualmente e extraia os arquivos.

2. **Configurar variáveis no secrets da aplicação**   
   - No mesmo diretório atual, entre no projeto de WebApi:
     ```sh
     cd auth-api/src/AuthApi.WebApi
     ```
      
   > **Observação importante:**  
   > As secrets no docker-compose.yml estão configuradas para rodar em ambiente Linux ou WSL. Caso esteja utilizando o Windows sem WSL, será necessário adaptar a configuração das secrets para garantir o funcionamento correto.

   - Execute os seguintes comandos para configurar as secrets necessários para o projeto AuthApi.WebApi:
      ```sh
      dotnet user-secrets init
      dotnet user-secrets set "JwtPrivateKey" "fedaf7d8863b48e197b9287d492b708e"
      dotnet user-secrets set "AuthDbConnectionString" "Server=auth_mysql;Port=3306;Database=auth_db;Uid=auth_user;Pwd=root123;"

      ---SECRETS HONEYCOMB
      dotnet user-secrets set "OTEL_SERVICE_NAME" "auth-api"
      dotnet user-secrets set "OTEL_EXPORTER_OTLP_PROTOCOL" "http/protobuf"
      dotnet user-secrets set "OTEL_EXPORTER_OTLP_ENDPOINT" "https://api.honeycomb.io"
      dotnet user-secrets set "OTEL_EXPORTER_OTLP_HEADERS" "x-honeycomb-team=SeuTokenDoHoneyComb"
      ```

4. **Iniciar os serviços Docker**
   - Volte um diretório onde está localizado o arquivo `docker-compose.yml`:
     ```sh
     cd ..
     ```
   - Utilize o seguinte comando para iniciar a API e os serviços necessários no Docker:
     ```sh
     docker compose -f docker-compose.yml up -d --force-recreate
     ```
   - Certifique-se de que todos os containers foram iniciados corretamente com:
     ```sh
     docker ps
     ```
      
   > **Rodando sem docker:**  
   > Para rodar localmente sem docker, é necessário iniciar somente a instância do MySql no docker-compose e iniciar o projeto normalmente com `dotnet run`. As variáveis de configurações estão localizadas no arquivo `appsettings.Development.json`.

5. **Base de dados**
   - A base é criada automaticamente com o nome conforme propriedade `MYSQL_DATABASE: auth_db` no docker-compose.
   - É necessário entrar no projeto `AuthApi.Infraestructure` e executar o comando do EF Core para criar a estrutura da base. 
     ```sh
     cd AuthApi.Infraestructure
     ```
     ```sh
     dotnet ef database update
     ```
---

## 🧪 Testes da aplicação

É necessário realizar alguns passos para testar o projeto.

1. **Acessar swagger da aplicação**
   - A API estará disponível na url `http://localhost:5300/swagger` iniciada pelo docker.

2. **Baixar a collection do postman**
   - A collection do postman está localizada na pasta `.doc/postman`. Nela contém os requests e os payloads dos mesmos.
