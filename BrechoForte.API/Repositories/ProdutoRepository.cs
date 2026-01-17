using BrechoForte.API.Data;
using BrechoForte.API.Models;
using Microsoft.EntityFrameworkCore; // Necessário para os métodos Async

namespace BrechoForte.API.Repositories
{
    // 1. A Herança (: IProdutoRepository)
    // Estamos dizendo: "Eu prometo cumprir todas as regras do contrato IProdutoRepository"
    public class ProdutoRepository : IProdutoRepository
    {
        // 2. Injeção de Dependência
        // O repositório precisa do Contexto para acessar o banco.
        private readonly BrechoContext _context;

        public ProdutoRepository(BrechoContext context)
        {
            _context = context;
        }

        public async Task<List<Produto>> BuscarTodos()
        {
            // Tradução: "Vá na tabela Produtos, pegue tudo e transforme numa Lista"
            return await _context.Produtos.ToListAsync();
        }

        public async Task<Produto?> BuscarPorId(int id)
        {
            // Tradução: "Vá na tabela Produtos e ache o PRIMEIRO que tenha o Id igual ao id que recebi"
            // Se não achar, retorna nulo (null).
            return await _context.Produtos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Produto> Adicionar(Produto produto)
        {
            // 1. Adiciona na memória (prepara a inserção)
            await _context.Produtos.AddAsync(produto);
            // 2. Comita no banco de dados (roda o INSERT INTO...)
            await _context.SaveChangesAsync();

            return produto;
        }

        public async Task<Produto> Atualizar(Produto produto, int id)
        {
            // Primeiro buscamos o produto original no banco
            Produto? produtoPorId = await BuscarPorId(id);

            if (produtoPorId == null)
            {
                throw new Exception($"Produto para o ID: {id} não foi encontrado no banco de dados.");
            }

            // Atualizamos os campos
            produtoPorId.Nome = produto.Nome;
            produtoPorId.Descricao = produto.Descricao;
            produtoPorId.Preco = produto.Preco;
            produtoPorId.Tamanho = produto.Tamanho;
            produtoPorId.EstaVendido = produto.EstaVendido;
            produtoPorId.FotoUrl = produto.FotoUrl;

            // Salvamos as alterações (UPDATE Produtos SET...)
            _context.Produtos.Update(produtoPorId);
            await _context.SaveChangesAsync();

            return produtoPorId;
        }

        public async Task<bool> Apagar(int id)
        {
            Produto? produtoPorId = await BuscarPorId(id);

            if (produtoPorId == null)
            {
                throw new Exception($"Produto para o ID: {id} não foi encontrado no banco de dados.");
            }

            // Remove do banco (DELETE FROM...)
            _context.Produtos.Remove(produtoPorId);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}