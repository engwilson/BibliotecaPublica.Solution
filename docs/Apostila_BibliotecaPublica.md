# Apostila — Projeto Biblioteca Pública

Este documento explica como o projeto foi criado, a escolha de ferramentas, a modelagem e os passos práticos para rodar e evoluir a aplicação. Use como material didático para aulas, documentação do repositório e compartilhamento no GitHub.

---

## Sumário
- Introdução: por que publicar no GitHub  
- Git e GitHub (passos práticos)  
- Escolha de ferramentas: Visual Studio vs Visual Studio Code — por que escolhemos `Visual Studio`  
- Arquitetura: MVC + DDD — motivação para DDD  
- Estrutura do projeto e explicação de código (modelos, `DbContext`, serviços, controllers, DTOs)  
- Entity Framework Core: ORM, `__Add-Migration__` e `__Update-Database__`  
- API: motivos e exemplo de uso comunitário (multilinguagem / povos originários / ODS)  
- Swagger: documentação e teste  
- Passos práticos: configurar, rodar, migrar, publicar  
- Boas práticas e próximos passos

---

## Introdução — por que publicar no GitHub
Publicar o projeto e as apostilas no GitHub:
- Permite colaboração (issues, PRs), versionamento e histórico de aprendizado.  
- Facilita a distribuição de material para comunidades, escolas e parceiros.  
- Serve como portfólio e registro didático das aulas e exercícios.

Recomendações:
- Crie um repositório público ou privado conforme a necessidade.
- Inclua `README.md`, `LICENSE` e esta apostila em `docs/`.

---

## Git e GitHub — passos essenciais
1. Inicializar repositório local:
   - `git init`
2. Criar `.gitignore` (usar template `VisualStudio`).
3. Primeiros commits:
   - `git add .`
   - `git commit -m "Initial commit — projeto Biblioteca Pública"`
4. Conectar ao GitHub:
   - `git remote add origin https://github.com/<usuario>/<repo>.git`
   - `git push -u origin main`
5. Fluxo recomendado:
   - Branch por feature: `git checkout -b feat/nome`
   - Pull Request para `main`/`master`
   - Reviews e CI (se possível)

Observação: mantenha credenciais (connection strings, secrets) fora do repositório — use `appsettings.Development.json` + __User Secrets__ (`dotnet user-secrets`) ou variáveis de ambiente.

---

## Ferramentas: Visual Studio vs Visual Studio Code
- `Visual Studio` (IDE completa):
  - Debug integrado, designers, suporte a projetos .NET grandes, ferramentas de banco de dados e integração com SQL Server/LocalDB, GUI para migrations via __Package Manager Console__, templates.
  - Escolhemos `Visual Studio` porque facilita depuração e produtividade em projetos empresariais/educacionais com múltiplos projetos (.WebApplication, .Data, .Service, .Domain).
- `Visual Studio Code` (editor leve):
  - Ótimo para multiplataforma, containers e devs que preferem linha de comando.
  - Recomendado para microservices e ambientes Linux/WSL.
  
Conclusão: uso do `Visual Studio` neste projeto para melhor experiência de ensino e integração de ferramentas.

---

## Arquitetura: MVC + DDD (por que DDD)
- MVC (Model-View-Controller) descreve a camada de apresentação (neste projeto: API controllers).
- DDD (Domain-Driven Design) foca no modelo de domínio e em representar regras/entidades do negócio:
  - Vantagens do DDD aqui: modelos (`Livro`, `Exemplar`, `Associado`, `Emprestimo`, `Autor`, `Editora`, `Filial`) refletem regras do domínio (ex.: contagem de exemplares, status do empréstimo).
  - Separação de responsabilidades: `BibliotecaPublica.Domain` (entidades), `BibliotecaPublica.Data` (persistência / `DbContext`), `BibliotecaPublica.Service` (regras e operações), `BibliotecaPublica.WebApplication` (API).
- Escolhemos DDD para manter o domínio explícito, favorecer testabilidade e permitir evolução independente das camadas técnicas.

---

## Estrutura do projeto e explicação de código
Estrutura principal:
- `BibliotecaPublica.Domain` — modelos e enums (linguagem em Português): ex.: `Livro`, `Autor`, `Associado`, `Emprestimo`, `Exemplar`, `Editora`, `Filial`.
  - Chaves: `Guid`.
  - Propriedades úteis: `NomeCompleto`, `EstaAtrasado`, etc.
- `BibliotecaPublica.Data` — Entity Framework Core `DbContext`:
  - Classe `BibliotecaPublicaDBContext : DbContext`.
  - `DbSet<Livro> Livros`, `DbSet<Emprestimo> Emprestimos`, etc.
  - `OnModelCreating(ModelBuilder)` configura tabelas, chaves, relacionamentos (incluindo many-to-many `Livro <-> Autor`) e conversões de `enum`.
  - Observação: para evitar erro do SQL Server sobre cascades múltiplos, o relacionamento de `Emprestimo.Livro` foi configurado com `OnDelete(DeleteBehavior.NoAction)`.
- `BibliotecaPublica.Service` — serviços que encapsulam operações assíncronas (CRUD) e lógica simples de inventário:
  - Ex.: `LivroService`, `ExemplarService`, `EmprestimoService`, `AssociadoService`, `AutorService`, `EditoraService`, `FilialService`.
  - Métodos padrões: `CriarAsync`, `ObterTodosAsync`, `ObterPorIdAsync`, `AtualizarAsync`, `RemoverAsync`.
  - Implementam carregamento de relacionamentos (`Include`) e atualizam contadores (ex.: `TotalExemplares`, `ExemplaresDisponiveis`).
- `BibliotecaPublica.WebApplication` — API:
  - Controllers por agregação (ex.: `LivroController`, `EmprestimoController`, etc.).
  - DTOs para entrada e saída (ex.: `LivroCreateDto`, `LivroDto`, `EmprestimoUpdateDto`) visando desacoplamento e segurança.
  - Registro de serviços e `DbContext` em `Program.cs`:
    ```csharp
    builder.Services.AddDbContext<BibliotecaPublicaDBContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<LivroService>();
    // ...
    ```
  - Swagger ativado em ambiente de desenvolvimento para testar rotas.

---

## Entity Framework Core — ORM, migrations e comandos
- ORM (Object-Relational Mapper): EF Core traduz entidades C# para tabelas/colunas SQL e gera SQL para CRUD.
- Passos para habilitar migrations:
  1. Instalar ferramenta (opcional CLI):
     - `dotnet tool install --global dotnet-ef --version 8.0.0`
  2. No projeto que contém o `DbContext` (ou especificando `--project`), criar migration:
     - CLI: `dotnet ef migrations add NomeDaMigracao`
     - Visual Studio (__Package Manager Console__): `__Add-Migration__ NomeDaMigracao`
  3. Aplicar migration ao banco:
     - CLI: `dotnet ef database update`
     - Visual Studio (__Package Manager Console__): `__Update-Database__`
- Observações:
  - Erro comum: SQL Server proíbe cascades em múltiplos caminhos. Se encontrar erro com FK e cascades, ajuste o `OnDelete` no `OnModelCreating` (ex.: `DeleteBehavior.NoAction` ou `SetNull`).
  - Se migration falhar parcialmente, geralmente remover a migration (`dotnet ef migrations remove`) e recriar é mais simples; em DB de desenvolvimento, dropar o banco e recriar pode ser rápido.

---

## API — por que escolher API e exemplo comunitário
Motivos para criar uma `Web API`:
- Interoperabilidade: qualquer cliente (web, mobile, desktop, outras linguagens) pode consumir a API via HTTP/JSON.
- Reutilização: dados e serviços podem ser usados por múltiplas aplicações e equipes.
- Educação: facilita que estudantes desenvolvam front-ends em diferentes linguagens e compartilhem com a comunidade.

Exemplo prático e social:
- Uma estudante de uma comunidade indígena pode criar um aplicativo em sua língua nativa (por exemplo, interface em `Tx`), consumindo nossa API para pesquisar obras, registrar empréstimos e contribuir com conteúdos locais.
- A API permite trocas de conhecimento e pode ajudar a cumprir Objetivos de Desenvolvimento Sustentável (ODS), como educação de qualidade (ODS 4) e reduções de desigualdades (ODS 10).

---

## Swagger — documentação automática
- O projeto inclui `Swashbuckle` e habilita Swagger em desenvolvimento.
- Ao executar a API local, acessar `/swagger` fornece UI para testar endpoints, ver DTOs e exemplos.
- Importante: documentar endpoints e DTOs facilita uso por terceiros (estudantes, parceiros).

---

## Passos práticos (setup rápido)
1. Ajuste `appsettings.json` / `appsettings.Development.json`:
   - Adicione `ConnectionStrings:DefaultConnection` apontando para o seu SQL Server / LocalDB.
2. Registrar `DbContext` e serviços (já presente em `Program.cs`).
3. Restaurar pacotes:
   - `dotnet restore` (ou build no Visual Studio).
4. Criar e aplicar migrations:
   - CLI:
     - `dotnet ef migrations add Inicial`
     - `dotnet ef database update`
   - OU no Visual Studio (__Package Manager Console__):
     - `__Add-Migration__ Inicial`
     - `__Update-Database__`
5. Rodar a API:
   - Via Visual Studio: pressione __F5__ (Start Debugging) ou sem debug (Start Without Debugging).
   - Via CLI: `dotnet run --project BibliotecaPublica.WebApplication`
6. Testar com Swagger: abra `https://localhost:<porta>/swagger`.

---

## Boas práticas e próximos passos recomendados
- Validação: adicione `DataAnnotations` ou `FluentValidation` nos DTOs.
- Mapeamento automático: usar `AutoMapper` para evitar mapeamento manual entre DTOs e entidades.
- Testes: criar testes unitários para serviços e testes de integração para controllers.
- Observability: adicionar logging estruturado e métricas.
- Segurança: implementar autenticação/ autorização (JWT) antes de publicar.
- Internacionalização: planejar suporte a conteúdo multilíngue para facilitar projetos comunitários.
- Documentação contínua: manter esta apostila em `docs/` atualizada com screenshots e exemplos de requests.

---

## Recursos úteis
- EF Core Migrations: https://learn.microsoft.com/ef/core/managing-schemas/migrations/  
- ASP.NET Core Web API: https://learn.microsoft.com/aspnet/core/web-api/  
- Swagger / Swashbuckle: https://github.com/domaindrivendev/Swashbuckle.AspNetCore  
- GitHub Guides: https://docs.github.com/
