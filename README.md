# CRUD Gerenciador de Produtos (API) + MySQL

API REST para gerenciamento de produtos com operações CRUD, construída com ASP.NET Core e Entity Framework Core, utilizando MySQL como banco de dados.

## Funcionalidades

- **Listar produtos** - Retorna produtos paginados com metadados de paginação
- **Buscar produto por ID** - Retorna um produto específico pelo ID
- **Criar produto** - Adiciona um novo produto com validação (retorna 201)
- **Atualizar produto** - Atualiza um produto existente
- **Deletar produto** - Remove um produto pelo ID
- **Documentação Swagger** - Interface interativa para testar a API
- **Tratamento centralizado de erros** - Respostas padronizadas para todos os tipos de erro

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Banco de dados MySQL/MariaDB (ver opções abaixo)
- Editor de código (VS Code, Visual Studio, etc.)

### Opções de Banco de Dados

#### Opção 1: XAMPP (Recomendado para iniciantes)

[XAMPP](https://www.apachefriends.org/) inclui MySQL, PHPMyAdmin e Apache prontos para uso.

1. Baixe e instale o [XAMPP](https://www.apachefriends.org/download.html)
2. Abra o Painel do XAMPP e inicie o **MySQL** (clique em "Start" ao lado de MySQL)
3. O MySQL estará rodando na porta padrão `3306`
4. Acesse o PHPMyAdmin em `http://localhost/phpmyadmin` para verificar se está funcionando

#### Opção 2: MySQL Standalone

Baixe e instale o [MySQL Server 8.0+](https://dev.mysql.com/downloads/mysql/) diretamente.

#### Opção 3: MariaDB

[MariaDB](https://mariadb.org/download/) é um fork do MySQL 100% compatível:

```bash
# macOS (Homebrew)
brew install mariadb
brew services start mariadb

# Ubuntu/Debian
sudo apt install mariadb-server
sudo systemctl start mariadb

# Windows - baixe o instalador em mariadb.org
```

#### Opção 4: Docker

```bash
docker run --name productmanager-mysql \
  -e MYSQL_ROOT_PASSWORD=123456 \
  -e MYSQL_DATABASE=ProductManagerDB \
  -p 3306:3306 \
  -d mysql:8.0
```

## Configuração

1. Clone o repositório:

```bash
git clone <url-do-repositorio>
cd crud-mysql-productmanager-api
```

2. Configure a string de conexão no `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ProductManagerDB;User=root;Password=sua_senha;"
  }
}
```

**Exemplos por tipo de banco:**

| Banco | Connection String |
|-------|-------------------|
| **XAMPP** (senha vazia) | `Server=localhost;Database=ProductManagerDB;User=root;Password=;` |
| **XAMPP** (senha definida) | `Server=localhost;Database=ProductManagerDB;User=root;Password=sua_senha;` |
| **MySQL Standalone** | `Server=localhost;Database=ProductManagerDB;User=root;Password=sua_senha;` |
| **MariaDB** | `Server=localhost;Database=ProductManagerDB;User=root;Password=sua_senha;` |
| **Docker** | `Server=localhost;Database=ProductManagerDB;User=root;Password=123456;` |
| **MySQL remoto** | `Server=192.168.1.100;Database=ProductManagerDB;User=meuuser;Password=1234;` |

> **Nota para XAMPP:** Por padrão o MySQL do XAMPP vem com usuário `root` e **senha vazia**. Se você definiu uma senha no phpMyAdmin, use-a na string de conexão.

3. Execute o script SQL para criar o banco e popular com dados de teste:

```bash
# Via linha de comando
mysql -u root -p < SQL_Code/ProductManagerDB.sql

# Ou via phpMyAdmin (XAMPP):
# 1. Acesse http://localhost/phpmyadmin
# 2. Clique na aba "SQL"
# 3. Cole o conteúdo do arquivo SQL_Code/ProductManagerDB.sql
# 4. Clique em "Executar"
```

> O script cria o banco `ProductManagerDB`, a tabela `Products` e insere 20 produtos de exemplo.

4. Execute o projeto:

```bash
dotnet run --project ProductManager.API
```

5. Acesse a API:

- **Swagger UI:** `https://localhost:7113/swagger`
- **API:** `https://localhost:7113/Product`

## Estrutura do Projeto

O projeto segue uma arquitetura em camadas (Clean Architecture):

```
ProductManager.API/          → Camada de apresentação (controllers, DTOs, middleware)
├── Controllers/             → Endpoints da API
├── Dtos/                    → Modelos de transferência de dados
├── Middleware/              → Tratamento centralizado de erros
└── Program.cs               → Configuração e inicialização

ProductManager.Domain/       → Camada de negócio (services, interfaces)
├── Interfaces/              → Contratos dos services
├── Services/                → Implementação da lógica de negócio
└── Extensions/              → Módulos de DI

ProductManager.Core/         → Modelos compartilhados
└── Models/                  → Entidades e modelos de paginação

ProductManager.Infra/        → Acesso a dados
├── Interfaces/              → Contratos dos repositories
├── Repository/              → Implementação com EF Core
├── Models/                  → DbContext e configurações
└── Extensions/              → Módulos de DI

SQL_Code/                    → Scripts de inicialização do banco
```

### Dependências entre camadas

```
API → Domain → Infra → Core
```

## Endpoints

| Método | Rota                | Descrição                        | Body                                          |
|--------|---------------------|----------------------------------|-----------------------------------------------|
| `GET`    | `/Product`            | Lista produtos paginados         | Query: `page`, `pageSize`                     |
| `GET`    | `/Product/{id}`       | Retorna um produto pelo ID       | -                                             |
| `POST`   | `/Product`            | Cria um novo produto             | `{ "nome": "string", "preco": 10.00 }`        |
| `PUT`    | `/Product/{id}`       | Atualiza um produto              | `{ "nome": "string", "preco": 10.00 }`        |
| `DELETE` | `/Product/{id}`       | Deleta um produto pelo ID        | -                                             |

### Exemplo de resposta paginada

```json
{
  "items": [
    { "id": 1, "nome": "Notebook Dell Inspiron 15", "preco": 4599.90 },
    { "id": 2, "nome": "Mouse Logitech MX Master 3S", "preco": 349.90 }
  ],
  "totalCount": 20,
  "totalPages": 2,
  "currentPage": 1,
  "pageSize": 10
}
```

### Formato de erro padronizado

```json
{
  "statusCode": 404,
  "message": "Produto com ID 99 não encontrado.",
  "timestamp": "2026-08-18T12:00:00Z"
}
```

## Conceitos Apresentados

Esta API é ideal para estudantes aprenderem os seguintes conceitos:

| Conceito | Onde aplicado |
|----------|---------------|
| **Clean Architecture** | Separação em camadas (API, Domain, Infra, Core) |
| **DTOs** | `ProductManager.API/Dtos/` - separação entre modelo de banco e API |
| **Padrão Repository** | `ProductManager.Infra/Repository/` |
| **Padrão Service** | `ProductManager.Domain/Services/` |
| **Injeção de Dependência** | Módulos de extensão em cada camada |
| **Paginação** | `PagedResult<T>` + query params no endpoint |
| **Tratamento de Erros** | `ExceptionMiddleware` com respostas padronizadas |
| **Validação de Dados** | Data Annotations no modelo e DTOs |
| **Entity Framework Core** | Mapeamento ORM, seed com `HasData` |
| **Serilog** | Logging estruturado em arquivo e console |
| **Swagger/OpenAPI** | Documentação interativa da API |

## Tecnologias

| Tecnologia             | Versão  | Descrição                          |
|------------------------|---------|------------------------------------|
| .NET                   | 10.0    | Framework de execução              |
| ASP.NET Core           | 10.0    | Framework web                      |
| Entity Framework Core  | 9.0     | ORM para acesso a dados            |
| Pomelo MySQL           | 9.0     | Provider EF Core para MySQL/MariaDB |
| Serilog                | 4.4     | Logging estruturado                |
| Swashbuckle            | 10.2    | Documentação Swagger/OpenAPI       |
| MySQL/MariaDB          | 8.0+    | Banco de dados relacional          |

## Rodando os Testes

```bash
dotnet test
```

- **36 testes** (unitários + integração)
- Usa InMemory Database (não precisa de MySQL para testar)
- Cobertura: Services, Controllers, Middleware, Paginação, CRUD

## Licença

Este projeto está licenciado sob a [Licença MIT](https://opensource.org/licenses/MIT).

## Autor

Wanfranklin Alves
