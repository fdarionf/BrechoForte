using System.ComponentModel.DataAnnotations;

namespace BrechoForte.API.DTOs
{
    // O que o usuário precisa mandar para cadastrar um produto
    public class AdicionarProdutoRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [MaxLength(100)]
        public string Nome { get; set; }

        [MaxLength(200)]
        public string Descricao { get; set; }

        public string Tamanho { get; set; }

        [Required]
        public decimal Preco { get; set; }
    }
}