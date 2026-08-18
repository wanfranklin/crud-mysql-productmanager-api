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
- [MySQL 8.0+](https://dev.mysql.com/downloads/mysql/)
- Editor de código (VS Code, Visual Studio, etc.)

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
    "DefaultConnection": "Server=localhost;Database=ProductManagerDB;User=root;Password=sua_senha"
  }
}
```

3. Execute o script SQL para criar o banco e popular com dados de teste:

```bash
mysql -u root -p < SQL_Code/ProductManagerDB.sql
```

> O script cria 20 produtos de exemplo nas categorias: Eletrônicos, Acessórios, Papelaria, Alimentos, Casa e Esportes.

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
| Entity Framework Core  | 10.0    | ORM para acesso a dados            |
| Pomelo MySQL           | 9.0     | Provider EF Core para MySQL        |
| Serilog                | 4.4     | Logging estruturado                |
| Swashbuckle            | 10.2    | Documentação Swagger/OpenAPI       |
| MySQL                  | 8.0+    | Banco de dados relacional          |

## Licença

Este projeto está licenciado sob a [Licença MIT](https://opensource.org/licenses/MIT).

## Autor

Wanfranklin Alves
