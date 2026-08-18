# CRUD Gerenciador de Produtos (API) + MySQL

API REST para gerenciamento de produtos com operações CRUD, construída com ASP.NET Core e Entity Framework Core, utilizando MySQL como banco de dados.

## Funcionalidades

- **Listar produtos** - Retorna todos os produtos cadastrados
- **Buscar produto por ID** - Retorna um produto específico pelo ID
- **Criar produto** - Adiciona um novo produto (retorna 201)
- **Atualizar produto** - Atualiza um produto existente
- **Deletar produto** - Remove um produto pelo ID
- **Buscar por nome** - Filtra produtos pelo nome
- **Buscar por faixa de preço** - Filtra produtos por preço mínimo e máximo
- **Documentação Swagger** - Interface interativa para testar a API

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

3. Execute o script SQL para criar o banco e a tabela:

```bash
mysql -u root -p < SQL_Code/ProductManagerDB.sql
```

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
ProductManager.API/          → Camada de apresentação (controllers, configuração)
ProductManager.Domain/       → Camada de negócio (services, interfaces)
ProductManager.Core/         → Modelos compartilhados (entidades)
ProductManager.Infra/        → Acesso a dados (repository, DbContext)
SQL_Code/                    → Scripts de inicialização do banco
```

### Dependências entre camadas

```
API → Domain → Infra → Core
```

## Endpoints

| Método | Rota                | Descrição                        | Body                                          |
|--------|---------------------|----------------------------------|-----------------------------------------------|
| `GET`    | `/Product`            | Lista todos os produtos          | -                                             |
| `GET`    | `/Product/{id}`       | Retorna um produto pelo ID       | -                                             |
| `POST`   | `/Product`            | Cria um novo produto             | `{ "nome": "string", "preco": 10.00 }`        |
| `PUT`    | `/Product/{id}`       | Atualiza um produto              | `{ "id": 1, "nome": "string", "preco": 10.00 }` |
| `DELETE` | `/Product/{id}`       | Deleta um produto pelo ID        | -                                             |

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
