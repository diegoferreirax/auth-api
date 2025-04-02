# Auth API

Projeto de autenticação e registro de usuários para fins de testes e treinamento.

## ⚙️ Configuração

Para configurar o ambiente de desenvolvimento, siga os passos abaixo:

1. **Baixar e instalar o Docker Desktop**
   - O projeto utiliza Docker para gerenciar os serviços necessários. Certifique-se de baixar e instalar o [Docker Desktop](https://www.docker.com/products/docker-desktop/) conforme o sistema operacional utilizado.

2. **Configurar o .NET 8**
   - Certifique-se de ter o [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) instalado.
   - Para verificar a instalação, execute:
     ```sh
     dotnet --version
     ```

3. **Obter Api Key para visualizar logs no Honeycomb (etapa não obrigatória)**   
   - O projeto utiliza o [Honeycomb](https://www.honeycomb.io/) para monitoramento e análise de logs.  
   - Para obter o token de API, siga os passos abaixo:  
     1. Acesse [Honeycomb.io](https://www.honeycomb.io/) e crie uma conta (caso ainda não tenha).  
     2. Após fazer login, vá até a seção **API Keys** no painel de configurações.  
     3. Gere um novo **API Key** e copie o valor gerado.  
     4. Esse token será necessário para configurar os secrets da aplicação nas próximas etapas.  


## 🛠️ Baixar e iniciar o projeto

1. **Baixar o projeto**
   - Clone o repositório utilizando o comando:
     ```sh
     git clone https://github.com/diegoferreirax/auth-api.git
     ```
   - Alternativamente, faça o download do código-fonte manualmente e extraia os arquivos.

2. **Configurar variáveis no secrets da aplicação**   
   - No mesmo diretório atual, entre no projeto de WebApi:
     ```sh
     cd auth-api\src\AuthApi.WebApi
     ```
   - Execute os seguintes comandos para configurar as secrets necessários para o projeto WebApi:
      ```sh
      dotnet user-secrets init
      dotnet user-secrets set "JwtPrivateKey" "fedaf7d8863b48e197b9287d492b708e"
      dotnet user-secrets set "AuthDatabase:ConnectionString" "mongodb://root:12345@auth_mongodb:27017"
      dotnet user-secrets set "AuthDatabase:DatabaseName" "auth_db"
      dotnet user-secrets set "AuthDatabase:DatabaseCollections:UsersCollection" "users"

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

5. **Base de dados**
   - A base de dados é criada automaticamente com o nome conforme passado na secret `AuthDatabase:DatabaseName`.

---

## 🚧 Testando o projeto

É necessário realizar alguns passos para testar o projeto.

1. **Acessar swagger da aplicação**
   - A API estará disponível na url `http://localhost:5300/swagger` iniciada pelo docker.

2. **Baixar a collection do postman**
   - A collection do postman está localizada na pasta `.doc/postman`. Nela contém os requests e os payloads dos mesmos.
