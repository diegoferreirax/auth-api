# Testes de Carga com JMeter

Este guia explica como executar testes de carga e performance na Auth API usando o JMeter.

## 📋 Pré-requisitos

1. **Instalar o JMeter**
   - Baixe o JMeter em [https://jmeter.apache.org/download_jmeter.cgi](https://jmeter.apache.org/download_jmeter.cgi)
   - Ou instale via package manager:
     ```bash
     # Windows (usando Chocolatey)
     choco install jmeter
     
     # Linux/Mac (usando Homebrew)
     brew install jmeter
     ```

2. **API rodando**
   - Certifique-se de que a API está rodando e acessível em `http://localhost:5300`

## 🚀 Executando os testes

### 1. Abrir o plano de testes

- Inicie o JMeter
- Abra o arquivo `auth-api.jmx` localizado nesta pasta (`.doc/jmeter/auth-api.jmx`)

### 2. Configurar as credenciais de autenticação (se necessário)

- O teste está configurado para autenticar com um usuário que tenha a role "UM" (User Manager)
- Por padrão, o email configurado é: `usermanager@gmail.com` que está configurado na base de dados
- Você pode alterar as credenciais no sampler "Autenticar Usuário" se necessário

### 3. Ajustar parâmetros de carga (opcional)

- **Threads (Usuários)**: 200 usuários virtuais simultâneos
- **Ramp-up Time**: 50 segundos (tempo para iniciar todas as threads)
- **Loops**: 2 iterações por thread
- Ajuste esses valores conforme necessário para seus testes

### 4. Fluxo de testes configurado

O plano de testes executa automaticamente o seguinte fluxo para cada thread:

- **Autenticar Usuário**: POST `/api/v1/users/authenticate`
  - Extrai o token JWT da resposta
- **Criar Usuário**: POST `/api/v1/users`
  - Usa o token no header Authorization
  - Extrai o ID do usuário criado
- **Atualizar Usuário**: PUT `/api/v1/users/{id}`
  - Usa o token e o ID extraído
- **Listar Usuários**: GET `/api/v1/users?PageNumber=1&PageSize=10`
  - Usa o token no header Authorization
  - Adiciona carga adicional à aplicação com queries de listagem
- **Deletar Usuário**: DELETE `/api/v1/users/{id}` (opcional, pode estar desabilitado)
  - Usa o token e o ID extraído

### 5. Executar o teste

**Via Interface Gráfica:**
- Clique no botão **▶️ Start** na barra de ferramentas do JMeter
- Ou use o menu: `Run → Start`

**Via Linha de Comando (modo headless):**

```bash
# Navegar até a pasta do projeto
cd auth-api

# Executar o teste
jmeter -n -t .doc/jmeter/auth-api.jmx -l results.jtl -e -o reports/

# Onde:
# -n: modo não-GUI (headless)
# -t: arquivo de teste
# -l: arquivo de log de resultados
# -e: gerar relatório HTML
# -o: diretório de saída do relatório HTML
```

Após a execução via linha de comando, abra o arquivo `reports/index.html` no navegador para visualizar o relatório detalhado.

### 6. Visualizar resultados

**Na Interface Gráfica:**
- **Árvore de Resultados**: Visualize cada requisição individualmente
- **Relatório de Sumário**: Estatísticas gerais dos testes
- **Gráfico de Resultados**: Visualização gráfica do desempenho

## ⚙️ Configurações atuais

- **Usuários simultâneos**: 200 threads
- **Ramp-up**: 50 segundos (~4 threads/segundo)
- **Iterações**: 2 loops por thread
- **Total de requisições**: ~2.000 requests (considerando o fluxo completo com 5 requests por iteração)

## 📊 Entendendo os resultados

### Durante o ramp-up (primeiros 50 segundos)
- Taxa de início: ~4 threads/segundo
- Cada thread executa 5 requests (Autenticar → Criar → Atualizar → Listar → Deletar)
- Durante o ramp-up: ~**20 requests por segundo** (4 threads × 5 requests)

### Após o ramp-up
- 200 threads ativas, cada uma executando 2 loops
- Total de requests por thread: 2 loops × 5 requests = **10 requests por thread**
- Total de requests: 200 threads × 10 requests = **2.000 requests**
- A frequência real dependerá do tempo de resposta da API

## 🔧 Personalização

Para ajustar o comportamento dos testes, edite o arquivo `auth-api.jmx`:

- **Aumentar carga**: Aumente o valor de `ThreadGroup.num_threads`
- **Aumentar velocidade**: Diminua o valor de `ThreadGroup.ramp_time`
- **Mais iterações**: Aumente o valor de `LoopController.loops`
- **Desabilitar delete**: O sampler "Deletar Usuário" pode ser desabilitado na interface do JMeter

## 🐛 Troubleshooting

**Erro ao executar o teste:**
- Verifique se a API está rodando em `http://localhost:5300`
- Verifique se o usuário `usermanager@gmail.com` existe no banco de dados
- Certifique-se de que o usuário tem a role "UM" (User Manager)

**Resultados não aparecem:**
- Verifique se os listeners (View Results Tree, Summary Report, etc.) estão habilitados
- Confira se há erros na aba "Log Viewer" do JMeter

**Erro de autenticação:**
- Verifique se as credenciais no sampler "Autenticar Usuário" estão corretas
- Certifique-se de que o usuário existe e tem as permissões necessárias

