using System.ComponentModel.DataAnnotations;

namespace BrechoForte.API.DTOs
{
    public class AtualizarProdutoRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [MaxLength(100)]
        public string Nome { get; set; }

        [MaxLength(200)]
        public string Descricao { get; set; }

        public string Tamanho { get; set; }

        [Required]
        public decimal Preco { get; set; }

        // Na atualização, permitimos corrigir se está vendido ou não
        public bool EstaVendido { get; set; }
    }
}