namespace BrechoForte.API.DTOs
{
    // O que devolvemos para a tela (aqui o ID existe)
    public class ProdutoResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Tamanho { get; set; }
        public decimal Preco { get; set; }
        public bool EstaVendido { get; set; }
    }
}