# CashFlowControl

**CashFlowControl** é uma aplicação para gerenciar o fluxo de caixa diário de comerciantes, registrando lançamentos de créditos e débitos e gerando um relatório consolidado de saldo diário. Desenvolvida em .NET Core e MongoDB, a aplicação segue princípios de arquitetura limpa e boas práticas para garantir escalabilidade, segurança e resiliência.

## Funcionalidades

- **Gerenciamento de Transações**: Adiciona lançamentos de crédito e débito com informações como data, valor e descrição.
- **Consolidação Diária de Saldo**: Gera um relatório consolidado do saldo para cada dia, incluindo o total de débitos e créditos.
- **Minimal API**: Interface para acesso e gerenciamento de transações e relatórios diários.
- **Escalabilidade e Resiliência**: Arquitetura preparada para crescimento e recuperação de falhas.

## Estrutura do Projeto
CashFlowControl/ 
  ├── CashFlowControl.Domain/ # Camada de domínio (entidades, serviços, interfaces de domínio) 
  ├── CashFlowControl.Application/ # Camada de aplicação (serviços, DTOs) 
  ├── CashFlowControl.Infrastructure/ # Camada de infraestrutura (repositórios, configuração de banco MongoDB) 
  ├── CashFlowControl.Presentation/ # Camada de apresentação (API) 
  ├── CashFlowControl.Tests/ # Testes automatizados para cada camada 
  └── README.md # Documentação e instruções de uso

## Requisitos

- **.NET 8 SDK**
- **MongoDB** (executando localmente ou usando MongoDB Atlas)
- **Minimal API**
- **SOLID**
- **Clean Code**
- **JWT**

## Configuração

### 1. Clonar o Repositório

```bash
git clone https://github.com/leonardo-francisco/CashFlowControl.git
cd CashFlowControl
```

### 2. Clonar o MongoDB
- **Localmente: Inicie o MongoDB em sua máquina**
- **Docker: Caso prefira usar Docker, você pode iniciar o MongoDB com o comando:
```bash
docker run -d -p 27017:27017 --name CashFlowControl mongo
```

### 3. Configuração do Arquivo appsettings.json
- **No diretório CashFlowControl.Presentation, crie um arquivo appsettings.json e configure a string de conexão do MongoDB**
```bash
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "CashFlowControlDB"
  },
  "JwtSettings": {
    "Secret": "fedaf7d8863b48e197b9287d492b708e",
    "Issuer": "https://seu-servidor-auth",
    "Audience": "seu-api"
  }
```

## Executando a Aplicação

### 1. Restaurar Dependências
- **No diretório raiz do projeto, execute:
```bash
dotnet restore
```

### 2. Rodar a Aplicação
- **Para iniciar a API, execute o seguinte comando:
```bash
dotnet run --project CashFlowControl.API
```
- **A aplicação será iniciada use ferramentas como Postman ou Swagger para testar os endpoints

### 3. Testando a Aplicação
- **Para rodar os testes:
```bash
dotnet test
```
- **Isso executará os testes em CashFlowControl.Test e exibirá os resultados no terminal

## Endpoints Principais

### Transações
- **POST /login**: Cria o token para autenticação das rotas
- **POST /transactions**: Adiciona uma nova transação (crédito ou débito).
- **GET /transaction/{date}**: Recupera uma transação pela data.
- **GET /consolidation/{date}**: Recupera um balanço consolidado pela data.
