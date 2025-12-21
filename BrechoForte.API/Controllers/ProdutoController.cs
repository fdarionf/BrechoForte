using BrechoForte.API.DTOs;
using BrechoForte.API.Models;
using BrechoForte.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BrechoForte.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository _repositorio;

        // Injetamos a INTERFACE, não a classe concreta.
        // Isso permite trocar o banco sem quebrar o controller.
        public ProdutoController(IProdutoRepository repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProdutoResponse>>> BuscarTodos()
        {
            List<Produto> produtos = await _repositorio.BuscarTodos();

            // Mapeamento Manual (De Entidade -> DTO)
            // Transformamos a lista de banco numa lista de vitrine
            var resposta = produtos.Select(p => new ProdutoResponse
            {
                Id = p.Id,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Preco = p.Preco,
                Tamanho = p.Tamanho,
                EstaVendido = p.EstaVendido,
                FotoUrl = p.FotoUrl
            }).ToList();

            return Ok(resposta);
        }

        // 3. BUSCAR POR ID (GET: api/Produto/5)
        // Essencial para ver detalhes de um único item
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponse>> BuscarPorId(int id)
        {
            var produto = await _repositorio.BuscarPorId(id);

            if (produto == null)
            {
                return NotFound(); // Retorna Erro 404 (Não Encontrado)
            }

            // Traduzindo Entidade -> DTO
            var resposta = new ProdutoResponse
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Tamanho = produto.Tamanho,
                EstaVendido = produto.EstaVendido,
                FotoUrl = produto.FotoUrl
            };

            return Ok(resposta);
        }

        // 4. ATUALIZAR (PUT: api/Produto/5)
        // Recebe o ID na URL e os dados no Corpo (JSON)
        [HttpPut("{id}")]
        public async Task<ActionResult<ProdutoResponse>> Atualizar(int id, [FromBody] AtualizarProdutoRequest request)
        {
            // Primeiro, transformamos o DTO na Entidade que o Repositório entende
            var produtoEditado = new Produto
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                Preco = request.Preco,
                Tamanho = request.Tamanho,
                EstaVendido = request.EstaVendido,
                FotoUrl = request.FotoUrl
            };

            try
            {
                // Chamamos o repositório para fazer a atualização
                var produtoAtualizado = await _repositorio.Atualizar(produtoEditado, id);

                // Traduzimos a resposta de volta para DTO
                var resposta = new ProdutoResponse
                {
                    Id = produtoAtualizado.Id,
                    Nome = produtoAtualizado.Nome,
                    Descricao = produtoAtualizado.Descricao,
                    Preco = produtoAtualizado.Preco,
                    Tamanho = produtoAtualizado.Tamanho,
                    EstaVendido = produtoAtualizado.EstaVendido,
                    FotoUrl = produtoAtualizado.FotoUrl
                };

                return Ok(resposta);
            }
            catch (Exception ex)
            {
                // Se o produto não existir (o repositório lança erro), devolvemos 404
                return NotFound(new { mensagem = "Produto não encontrado para atualizar" });
            }
        }

        // 5. DELETAR (DELETE: api/Produto/5)
        [HttpDelete("{id}")]
        public async Task<ActionResult> Apagar(int id)
        {
            try
            {
                var sucesso = await _repositorio.Apagar(id);

                // No Delete, geralmente retornamos "NoContent" (204)
                // Significa: "Deu certo, mas não tenho nada pra te mostrar, afinal sumiu."
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { mensagem = "Produto não encontrado para deletar" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoResponse>> Cadastrar([FromBody] AdicionarProdutoRequest request)
        {
            // Mapeamento Manual (De DTO -> Entidade)
            var novoProduto = new Produto
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                Preco = request.Preco,
                Tamanho = request.Tamanho,
                FotoUrl = request.FotoUrl,
                EstaVendido = false // Padrão
            };

            // Manda o repositório salvar
            var produtoCriado = await _repositorio.Adicionar(novoProduto);

            // Mapeamento de volta (Entidade -> DTO de Resposta)
            var resposta = new ProdutoResponse
            {
                Id = produtoCriado.Id,
                Nome = produtoCriado.Nome,
                Descricao = produtoCriado.Descricao,
                Preco = produtoCriado.Preco,
                Tamanho = produtoCriado.Tamanho,
                EstaVendido = produtoCriado.EstaVendido
            };

            // Retorna 201 Created
            return CreatedAtAction(nameof(BuscarTodos), new { id = resposta.Id }, resposta);
        }

        

    }
}