# Biblioteca Pública — README

Resumo rápido
- Projeto .NET 8 para gerenciar uma biblioteca pública (API REST).
- Arquitetura: DDD (Domain), Data (EF Core), Service (lógica), WebApplication (API).
- Ferramenta recomendada: Visual Studio 2022 (depuração e produtividade).

Estrutura de pastas
- `BibliotecaPublica.Domain` — entidades e enums.
- `BibliotecaPublica.Data` — `DbContext`, migrations (EF Core).
- `BibliotecaPublica.Service` — serviços de domínio (CRUD).
- `BibliotecaPublica.WebApplication` — controllers, DTOs, Swagger, `Program.cs`.

Pré-requisitos
- .NET 8 SDK
- SQL Server (LocalDB, SQL Express ou outro)
- Visual Studio 2022 (recomendado) ou VS Code + CLI
- (Opcional) dotnet-ef para CLI de migrations

Configuração inicial (CLI)
1. Clonar repositório
   - git clone https://github.com/engwilson/BibliotecaPublica.Solution.git
2. Entrar na solução
   - cd BibliotecaPublica.Solution
3. Restaurar pacotes
   - dotnet restore

Connection string
- Defina a connection string em `BibliotecaPublica.WebApplication\appsettings.Development.json` com a chave `ConnectionStrings:DefaultConnection` ou use __User Secrets__:
  - Exemplo mínimo:
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BibliotecaPublicaDb;Trusted_Connection=True;MultipleActiveResultSets=true"
      }
    }
    ```

EF Core (migrations)
- Instalar ferramenta CLI (opcional):
  - dotnet tool install --global dotnet-ef --version 8.0.0
- Criar migration (executar no projeto `BibliotecaPublica.Data` ou informar `--project` e `--startup-project`):
  - CLI: `dotnet ef migrations add Inicial --project BibliotecaPublica.Data --startup-project BibliotecaPublica.WebApplication`
  - Visual Studio (__Package Manager Console__): `__Add-Migration__ Inicial -Project BibliotecaPublica.Data -StartupProject BibliotecaPublica.WebApplication`
- Aplicar migration ao banco:
  - CLI: `dotnet ef database update --project BibliotecaPublica.Data --startup-project BibliotecaPublica.WebApplication`
  - Visual Studio (__Package Manager Console__): `__Update-Database__ -Project BibliotecaPublica.Data -StartupProject BibliotecaPublica.WebApplication`

Observação sobre erro comum
- SQL Server pode recusar cascades que criem múltiplos caminhos em cascata. Se receber erro relacionado a FK/cascata, veja que o `DbContext` já aplica `DeleteBehavior.NoAction` para `Emprestimo.Livro` para evitar esse problema.

Executando a API
- No Visual Studio: abra a solução e use __Start Debugging__ (F5) ou __Start Without Debugging__.
- Via CLI:
  - dotnet run --project BibliotecaPublica.WebApplication

Testes e documentação (Swagger)
- Em ambiente de desenvolvimento a UI do Swagger está habilitada: acesse `https://localhost:{porta}/swagger` para testar endpoints e ver contratos (DTOs).

Comandos Git essenciais
- Criar branch de feature:
  - git checkout -b feat/nome-da-feature
- Commitar e enviar:
  - git add .
  - git commit -m "Descrição"
  - git push origin feat/nome-da-feature
- Abrir Pull Request no GitHub para revisão.

Boas práticas
- Não commite secrets ou connection strings em repositórios públicos — use __User Secrets__ ou variáveis de ambiente.
- Valide DTOs com DataAnnotations ou FluentValidation antes de persistir.
- Use `AutoMapper` para simplificar mapeamentos entre entidades e DTOs (opcional).
- Adicione testes unitários e testes de integração para os serviços e controllers.

Exemplo rápido de uso (curl)
- Listar autores:
  - curl -s https://localhost:{porta}/api/Autor | jq
- Criar autor:
  - curl -X POST -H "Content-Type: application/json" -d '{"PrimeiroNome":"João","Sobrenome":"Silva","Biografia":"..."}' https://localhost:{porta}/api/Autor

Recursos úteis
- EF Core Migrations: https://learn.microsoft.com/ef/core/managing-schemas/migrations/
- ASP.NET Core Web API: https://learn.microsoft.com/aspnet/core/web-api/
- Swagger / Swashbuckle: https://github.com/domaindrivendev/Swashbuckle.AspNetCore

Contribuição e material didático
- Use este repositório para compartilhar apostilas, exercícios e exemplos com a comunidade. Mantenha o `docs/` com materiais (a apostila já disponível em `docs/Apostila_BibliotecaPublica.md`).

Contato
- Abra uma _issue_ ou _pull request_ no GitHub para sugestões, correções ou dúvidas.