# Ganho de Capital

Aplicação para cálculo de imposto sobre ganho de capital em operações de compra e venda de ações.

## Executando com Docker

Este projeto pode ser executado usando Docker, sem a necessidade de instalar o .NET diretamente na sua máquina.

### Pré-requisitos

- [Docker](https://www.docker.com/products/docker-desktop)
- [Docker Compose](https://docs.docker.com/compose/install/) (geralmente já vem com o Docker Desktop)

### Iniciando o ambiente

1. Clone o repositório
2. Navegue até a pasta do projeto
3. Execute o comando:

```bash
docker-compose up -d
```

Isso irá:
- Criar um container com o SDK do .NET 6.0
- Montar o código fonte dentro do container
- Restaurar as dependências
- Compilar o projeto

### Executando a aplicação

Para executar a aplicação console, use:

```bash
docker exec -it ganho-de-capital-app dotnet run --project GanhoDeCapital.ConsoleApp
```

### Executando os testes

Para executar todos os testes, use:

```bash
docker exec -it ganho-de-capital-app dotnet test
```

Para executar apenas os testes de um projeto específico:

```bash
docker exec -it ganho-de-capital-app dotnet test GanhoDeCapital.Tests
```

### Acessando o shell do container

Se precisar acessar o shell do container para executar comandos adicionais:

```bash
docker exec -it ganho-de-capital-app bash
```

### Parando o ambiente

Para parar os containers:

```bash
docker-compose down
```

## Estrutura do Projeto

- **GanhoDeCapital.ConsoleApp**: Aplicação console para interação com o usuário
- **GanhoDeCapital.Domain**: Contém as entidades, regras de negócio e lógica de domínio
- **GanhoDeCapital.Application**: Camada de aplicação que coordena as operações de domínio
- **GanhoDeCapital.Tests**: Testes unitários e de integração