# Minimal API - Gerenciamento de Veículos e Administradores 🚀

Este projeto, construído em **.NET 9** com **Minimal API**, é uma solução robusta para o gerenciamento de veículos e usuários. A branch `main` está configurada para **desenvolvimento e testes locais**, utilizando o **SQL Server Express LocalDB** para uma experiência ágil.

As funcionalidades de segurança foram aprimoradas com **hashing de senhas** para armazenamento seguro e as validações de dados foram refatoradas utilizando **FluentValidation** para um código mais limpo e manutenível.

## 🌳 Estrutura de Branches

Este repositório utiliza duas branches principais com propósitos distintos:

-   **`main` (Esta Branch):** Destinada ao desenvolvimento e testes locais. Contém as funcionalidades mais recentes e estáveis. Utiliza configurações locais (`appsettings.Development.json`) e se conecta a um banco de dados **SQL Server LocalDB**.
-   **`prod`:** A branch de produção. Todo push para esta branch aciona um workflow de **CI/CD (GitHub Actions)** que faz o deploy automático para o **Azure App Service**. Utiliza o **Azure SQL Database** e lê configurações seguras (secrets) diretamente do ambiente do Azure.

## 🧾 Funcionalidades Principais

### Gerenciamento de Veículos
- `GET /veiculos`: Lista todos os veículos.
- `GET /veiculos/{id}`: Busca um veículo por ID.
- `POST /veiculos`: Insere um novo veículo.
- `PUT /veiculos/{id}`: Atualiza um veículo existente.
- `DELETE /veiculos/{id}`: Remove um veículo.

### Gerenciamento de Administradores
- `GET /administradores`: Lista todos os administradores.
- `GET /administradores/{id}`: Busca um administrador por ID.
- `POST /administradores`: Insere um novo administrador.
- `POST /administradores/login`: Realiza o login, gerando um token JWT.

## 🛡️ Segurança e Validação

-   **Autenticação JWT:** Acesso seguro às rotas via tokens JWT com controle de acesso baseado em roles (`Adm` ou `Editor`).
-   **Hashing de Senhas:** Nenhuma senha é armazenada em texto plano. Utilizamos o `PasswordHasher` do ASP.NET Core para garantir que apenas hashes seguros sejam persistidos no banco de dados.
-   **Validação com FluentValidation:** As regras de validação de DTOs foram desacopladas dos endpoints e centralizadas em classes de validadores, tornando o código mais limpo, reutilizável e fácil de manter.

## 🛠️ Tecnologias Utilizadas

-   **Plataforma:** .NET 9
-   **Framework:** Minimal API
-   **ORM:** Entity Framework Core
-   **Banco de Dados (Local):** SQL Server Express LocalDB
-   **Validação:** FluentValidation
-   **Autenticação:** JWT
-   **Documentação:** Swagger
-   **Banco de Dados (Produção):** Azure SQL Database
-   **Hospedagem:** Azure App Service
-   **CI/CD:** GitHub Actions

## 💻 Como Executar e Testar Localmente (Branch `main`)

Siga os passos abaixo para configurar e rodar o projeto na sua máquina.

### Pré-requisitos
1.  **SDK do .NET 9** instalado.
2.  **SQL Server Express LocalDB** instalado (geralmente vem com o Visual Studio ou pode ser instalado separadamente).


### Passo 1: Clone o Repositório
```bash
git clone [https://github.com/levNicolaDev/minimal-api.git](https://github.com/levNicolaDev/minimal-api.git)
cd minimal-api
```

### Passo 2: Configure o Ambiente Local
Crie um arquivo chamado `appsettings.Development.json` na raiz do projeto `Api` com o seguinte conteúdo:

```JSON
{
  "ConnectionStrings": {
    "AzureConnection": "Server=(localdb)\\mssqllocaldb;Database=EFDesignDB;Trusted_Connection=True;"
  },
  "Jwt": {
    "Key": "MINHA_CHAVE_SECRETA_PARA_DESENVOLVIMENTO_DEVE_SER_BEM_LONGA_E_SEGURA_12345"
  },
  "AppSettings": {
    "AdminDefaultPassword": "UmaSenhaForteParaAdm!Local123"
  }
}
```
⚠️ Importante: Este arquivo contém segredos e já está no `.gitignore` para não ser enviado ao repositório.

### Passo 3: Crie e Popule o Banco de Dados
No terminal, na pasta do projeto `Api`, execute os seguintes comandos do Entity Framework Core:

**1. Criar as migrations (se a pasta `Migrations` não existir):**

```Bash
dotnet ef migrations add InitialCreate
```

**2. Aplicar as migrations para criar as tabelas no LocalDB:**

```Bash
dotnet ef database update
```

### Passo 4: Execute a Aplicação
Execute o comando abaixo para iniciar a API. Ao iniciar, a aplicação criará automaticamente o usuário administrador padrão com a senha definida no `appsettings.Development.json`.

```Bash
dotnet run
```

### Passo 5: Teste na Documentação Swagger
Acesse a URL informada no console (geralmente `http://localhost:5066`) e navegue para `/swagger`.

1. Use o endpoint `POST /administradores/login`.

2. No corpo da requisição, use as credenciais:
 - Email: `administrador@teste.com`
 - Senha: `UmaSenhaForteParaAdm!Local123`

3. Copie o token JWT retornado.

4. Clique no botão "Authorize" no topo da página e cole o token `Bearer`.

5. Agora você pode testar todos os endpoints protegidos!

## 📄 Acessar a Documentação

Para explorar a versão da API que está em produção, acesse o link abaixo. Esta versão está conectada ao Azure SQL Database e foi implantada via GitHub Actions.

👉 **[https://minimalapi.azurewebsites.net/swagger](https://minimalapi-abdcc3brghavbsdp.brazilsouth-01.azurewebsites.net/swagger)**
