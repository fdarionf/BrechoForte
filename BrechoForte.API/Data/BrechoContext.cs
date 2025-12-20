using BrechoForte.API.Models; // 1. Importamos os Models para ele enxergar a classe 'Produto'
using Microsoft.EntityFrameworkCore; // 2. Importamos o EF Core para ter os poderes de banco

namespace BrechoForte.API.Data
{
    // A classe herda de 'DbContext'.
    // Isso transforma uma classe comum em um "Gerente de Banco de Dados".
    public class BrechoContext : DbContext
    {
        // CONSTRUTOR
        // Ele serve para receber as configurações (como a senha do banco) lá do Program.cs
        // e passar para a classe pai (base) configurar o motor do EF Core.
        public BrechoContext(DbContextOptions<BrechoContext> options) : base(options)
        {
        }

        // TABELAS
        // Aqui dizemos: "O meu banco deve ter uma tabela de Produtos".
        // DbSet = Representação da Tabela.
        public DbSet<Produto> Produtos { get; set; }
    }
}