namespace BrechoForte.API.Models
{
    public class Configuracao
    {
        public int Id { get; set; }
        public string NomeLoja { get; set; } = "Loja Nova"; // Valor padrão
        public string SenhaAdmin { get; set; } = "Padrao@123";
        public string Whatsapp { get; set; } = "5551999999999";

    }
}