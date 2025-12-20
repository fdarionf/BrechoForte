# 📘 Manual Técnico: Projeto DarioBrecho API

## 1. O Fluxo da Aplicação
Entenda o caminho que os dados fazem quando alguém clica em "Salvar".

1. **Cliente/Swagger** envia JSON (Request).
2. **Controller** recebe, valida e converte (DTO -> Entidade).
3. **Repository** é chamado e usa o EF Core.
4. **DbContext** traduz para SQL.
5. **SQL Server** executa e retorna os dados.
6. **Controller** converte a resposta (Entidade -> DTO) e devolve para a tela.

---

## 2. Dicionário de Arquivos (Quem é Quem)

### 🏗️ Configuração e Infraestrutura

* **`Program.cs` (O Maestro):**
    * **O que faz:** É o ponto de partida. Configura a "Injeção de Dependência".
    * **Importância:** É aqui que ensinamos ao sistema que `IProdutoRepository` deve usar a classe `ProdutoRepository`, e onde configuramos a conexão com o banco.
* **`appsettings.json` (O Arquivo de Segredos):**
    * **O que faz:** Guarda configurações que não são código, como a **ConnectionString** (endereço e senha do banco).
    * **Importância:** Permite mudar o banco de dados sem precisar recompilar o código.
* **`BrechoContext.cs` (A Ponte):**
    * **O que faz:** Herda de `DbContext`. É o tradutor oficial entre C# e SQL.
    * **Importância:** Se não estiver aqui (`DbSet<Produto>`), a tabela não existe pro sistema.

### 🧠 Regras de Negócio e Dados

* **`Models/Produto.cs` (A Entidade/Molde):**
    * **O que faz:** Representa a tabela do banco de dados fielmente.
    * **Detalhe:** Usa *Data Annotations* (`[Key]`, `[Required]`) para definir regras do banco.
* **`DTOs/` (Os Carteiros Blindados):**
    * `AdicionarProdutoRequest`: Filtra o que entra (impede hack de preço ou ID).
    * `ProdutoResponse`: Filtra o que sai (esconde dados sensíveis ou loops).
    * **Importância:** Segurança e estabilidade do contrato da API.

### 🎮 Controle e Execução

* **`Repositories/IProdutoRepository.cs` (O Contrato):**
    * **O que faz:** Lista as promessas (Interface) do que pode ser feito, sem dizer como.
    * **Importância:** Permite trocar a tecnologia de banco no futuro sem quebrar o resto.
* **`Repositories/ProdutoRepository.cs` (O Operário):**
    * **O que faz:** Cumpre o contrato. Suja as mãos com o `DbContext` para rodar comandos no banco (`Add`, `Update`, `Remove`).
* **`Controllers/ProdutoController.cs` (O Gerente):**
    * **O que faz:** Atende o cliente (HTTP).
    * **Tarefas:** Recebe o pedido -> Valida -> Chama o Repositório -> Devolve a resposta (200 OK, 404 Not Found). **Não** acessa o banco diretamente.

---

## 3. Glossário de Termos Técnicos

| Termo | Significado Simplificado |
| :--- | :--- |
| **API REST** | Um sistema que conversa via HTTP (Web) usando verbos padrão (GET, POST, PUT, DELETE). |
| **CRUD** | Create (Criar), Read (Ler), Update (Atualizar), Delete (Apagar). O básico de qualquer sistema. |
| **Entity Framework (EF Core)** | A ferramenta que traduz código C# para comandos SQL automaticamente (ORM). |
| **Migration** | O histórico de evolução do banco. Transforma suas classes C# em tabelas reais. |
| **Injeção de Dependência (DI)** | Técnica onde o `Program.cs` cria e entrega as classes prontas, em vez de você usar `new` em todo lugar. |
| **Swagger** | A página web azul que gera a documentação e permite testar a API visualmente. |
| **Endpoint** | Cada "botão" ou URL disponível na sua API (ex: `/api/Produto`). |

---

## 4. Cheat Sheet (Guia Rápido de Criação do Zero)

Se for começar um projeto novo, siga esta ordem exata para não ter erro:

### 1. Preparação (Instalar Pacotes)
Antes de codar, abra o Terminal e instale as ferramentas do Banco de Dados:
* `Microsoft.EntityFrameworkCore`
* `Microsoft.EntityFrameworkCore.SqlServer`
* `Microsoft.EntityFrameworkCore.Tools`
* `Microsoft.EntityFrameworkCore.Design`

### 2. A Base (Banco de Dados)
1. **Model:** Crie a classe (ex: `Produto.cs`) com propriedades e `[Key]`.
2. **Context:** Crie o `AppDbContext.cs` herdando de `DbContext`.
3. **Connection String:** No `appsettings.json`, adicione a linha do banco.
4. **Program.cs:** Adicione o `builder.Services.AddDbContext...` para ligar tudo.

### 3. Criação Real do Banco (Migrations)
Sempre que mexer no **Model**, rode no Console do Gerenciador de Pacotes:
1. `Add-Migration NomeDaMudanca` (Cria o script)
2. `Update-Database` (Aplica no banco)

### 4. A Lógica (Back-end)
1. **Interface:** Crie `IRepository.cs` (Defina os métodos).
2. **Repository:** Crie `Repository.cs` (Implemente o acesso ao banco).
3. **Program.cs:** Registre a injeção: `builder.Services.AddScoped<IInterface, Classe>();`.

### 5. A Exposição (API)
1. **DTOs:** Crie os `Requests` (sem ID) e `Responses`.
2. **Controller:** Crie o Controller, injete o Repositório no construtor e crie os métodos (GET, POST, PUT, DELETE).

---