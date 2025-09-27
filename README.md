# Minimal API - Gerenciamento de Veículos e Administradores (em desenvolvimento 👷🏻)

Este projeto foi desenvolvido em **.NET 9** utilizando a arquitetura **Minimal API** e se destina ao gerenciamento de veículos e administradores. Ele utiliza o **Entity Framework Core** para persistência de dados em um banco de dados **MySQL** e implementa um sistema robusto de autenticação e autorização via **JWT**, com controle de acesso baseado em roles (Administrador e Editor).

## 🧾 Funcionalidades Principais

### Gerenciamento de Veículos

- `GET /veiculos`: Lista todos os veículos cadastrados, com suporte opcional a paginação.
- `GET /veiculos/{id}`: Busca um veículo específico pelo seu ID.
- `POST /veiculos`: Insere um novo veículo no banco de dados.
- `PUT /veiculos/{id}`: Atualiza os dados de um veículo existente.
- `DELETE /veiculos/{id}`: Remove um veículo do sistema.

### Gerenciamento de Administradores

- `GET /administradores`: Lista todos os administradores cadastrados.
- `GET /administradores/{id}`: Busca um administrador específico pelo seu ID.
- `POST /administradores`: Insere um novo administrador.
- `POST /administradores/login`: Realiza o login, gerando um token JWT para autenticação.

## 🔒 Segurança e Acesso

- **Autenticação e Autorização:** Implementada via JWT para garantir acesso seguro às rotas.
- **Controle de Acesso (Roles):** As rotas são protegidas, permitindo o acesso apenas a usuários com as roles **Adm** ou **Editor**, conforme a permissão necessária para cada funcionalidade.

## 🚀 Tecnologias Utilizadas

- **Plataforma:** .NET 9
- **Framework:** Minimal API
- **ORM:** Entity Framework Core
- **Banco de Dados:** MySQL
- **Autenticação:** JWT
- **Documentação:** Swagger

## 🛠️ Instruções de Uso

### 1. Requisitos Prévios

- **[.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)**
- **[MySQL Server](https://www.mysql.com/downloads/)**
- **[MySQL Workbench](https://www.mysql.com/products/workbench/)** (opcional)


### 2. Configuração do Banco de Dados

1. Crie um banco de dados chamado `minimal_api` no seu servidor MySQL.
2. No arquivo `appsettings.json` do projeto, insira a sua string de conexão:
   ```json
   "ConnectionStrings": {
     "MySql": "Server=localhost;Database=minimal_api;Uid=root;Pwd=sua_senha;"
   }
   ```

   
### 3. Execução das Migrações

Abra o terminal na pasta do projeto e execute:

```bash
dotnet ef database update
```


### 4. Executando a API

Para iniciar o servidor, use o comando:

```bash
dotnet run
```
A API estará rodando em `https://localhost:[sua porta]`. A documentação Swagger pode ser 
acessada em `https://localhost:[sua porta]/swagger`.

