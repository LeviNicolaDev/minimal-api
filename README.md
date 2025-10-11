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
- **Segredos na Nuvem:** A Connection String e a JWT Key são lidas com segurança através das variáveis de ambiente do Azure App Service, e não são expostas no código.

## 🚀 Tecnologias Utilizadas

- **Plataforma:** .NET 9
- **Framework:** Minimal API
- **ORM:** Entity Framework Core
- **Banco de Dados:** MySQL
- **Autenticação:** JWT
- **Documentação:** Swagger
- **Banco de Dados:** Azure SQL Database
- **Hospedagem:** Azure App Service
- **CI/CD:** GitHub Actions

## 📄 Acessar a Documentação

Para explorar todos os endpoints da API, acesse o link abaixo:

👉 **[https://minimalapi.azurewebsites.net/swagger](https://minimalapi-abdcc3brghavbsdp.brazilsouth-01.azurewebsites.net/swagger)**
