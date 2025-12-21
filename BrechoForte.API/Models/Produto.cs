using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrechoForte.API.Models
{
    public class Produto
    {
        [Key] // Define que esta é a Chave Primária (PK)
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório")]
        [MaxLength(100)] // Define varchar(100) no banco
        public string Nome { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Descricao { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Tamanho { get; set; } = string.Empty; // Ex: P, M, G, 42

        // IMPORTANTE: Dinheiro sempre usa decimal com precisão definida
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Preco { get; set; }

        public bool EstaVendido { get; set; } = false;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public string? FotoUrl { get; set; }
        
    }
}