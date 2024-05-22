# CRUD Gerenciador de Produtos (API) + MySQL

Este é um projeto EM EVOLUÇÃO que demonstra operações CRUD (Create, Read, Update, Delete) em um banco de dados MySQL usando C# e .NET.

## Funcionalidades

- Lista produto por ID (Por enquanto, mocado).

## Configuração

Certifique-se de ter o .NET 8 SDK e o MySQL instalados na sua máquina.

### Execução do projeto

- Clone o repositório para a sua máquina local.
- Abra o projeto no Visual Studio Code ou em qualquer outro editor de código de sua preferência.
- Certifique-se de configurar corretamente a string de conexão e o nome do banco de dados no arquivo `appsettings.json`.

- Execute o projeto.

## Estrutura do Projeto

O projeto está estruturado da seguinte forma:

- `API`
- `Domain`
- `Core`
- `Infrastructure`

## Documentação da API

#### Retorna todos os itens

```http
  GET /api/items
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `api_key` | `string` | **Obrigatório**. A chave da sua API |

#### Retorna um item

```http
  GET /api/items/${id}
```

| Parâmetro   | Tipo       | Descrição                                   |
| :---------- | :--------- | :------------------------------------------ |
| `id`      | `string` | **Obrigatório**. O ID do item que você quer |

#### add(num1, num2)

Recebe dois números e retorna a sua soma.

## Licença

Este projeto está licenciado sob a [Licença MIT](https://opensource.org/licenses/MIT).

## Autor

Wanfranklin Alves
