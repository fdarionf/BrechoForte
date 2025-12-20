using BrechoForte.API.Models;

namespace BrechoForte.API.Repositories
{
    // A Interface é um CONTRATO.
    // Ela lista O QUE o repositório deve fazer, mas não COMO.
    public interface IProdutoRepository
    {
        // Promessa: Vou te devolver uma lista de produtos
        Task<List<Produto>> BuscarTodos();

        // Promessa: Vou buscar um produto pelo ID
        Task<Produto?> BuscarPorId(int id);

        // Promessa: Vou adicionar um produto novo
        Task<Produto> Adicionar(Produto produto);

        // Promessa: Vou atualizar um produto existente
        Task<Produto> Atualizar(Produto produto, int id);

        // Promessa: Vou deletar um produto
        Task<bool> Apagar(int id);
    }
}