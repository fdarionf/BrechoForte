using BrechoForte.API.Data; // IMPORTANTE: Para achar o BrechoContext
using BrechoForte.API.Repositories;
using Microsoft.EntityFrameworkCore; // IMPORTANTE: Para achar o UseSqlServer

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- INÍCIO DA NOSSA CONFIGURAÇÃO DO BANCO ---

// Estamos "injetando" o BrechoContext na memória da aplicação.
// Dizemos: "Use o SQL Server e pegue a senha/endereço lá do arquivo appsettings.json"
builder.Services.AddDbContext<BrechoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- FIM DA NOSSA CONFIGURAÇÃO DO BANCO ---

// Configuração da Injeção de Dependência do Repositório
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

// 1. Configurar o CORS (Permitir que qualquer site acesse nossa API por enquanto)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
    {
        policy.AllowAnyOrigin()  // Aceita pedidos de qualquer lugar
              .AllowAnyMethod()  // Aceita GET, POST, DELETE...
              .AllowAnyHeader(); // Aceita qualquer cabeçalho
    });
});

var app = builder.Build();

// Configura o pipeline de requisições HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 2. Ativar a porteira que criamos acima
app.UseCors("PermitirTudo");

app.UseAuthorization();

app.MapControllers();

app.Run();