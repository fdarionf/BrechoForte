using BrechoForte.API.Data; // <--- Onde mora o BrechoContext
using BrechoForte.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BrechoForte.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracaoController : ControllerBase
    {
        // 👇 AQUI ERA O ERRO: Troque AppDbContext por BrechoContext
        private readonly BrechoContext _context;

        public ConfiguracaoController(BrechoContext context)
        {
            _context = context;
        }

        // ... O resto do código continua igual ...

        [HttpGet]
        public ActionResult<Configuracao> ObterConfiguracao()
        {
            var config = _context.Configuracoes.FirstOrDefault();

            if (config == null)
            {
                config = new Configuracao();
                _context.Configuracoes.Add(config);
                _context.SaveChanges();
            }

            return Ok(config);
        }

        [HttpPut]
        public IActionResult AtualizarConfiguracao([FromBody] Configuracao dadosNovos)
        {
            var config = _context.Configuracoes.FirstOrDefault();

            if (config == null) return NotFound("Configuração não encontrada.");

            if (!string.IsNullOrEmpty(dadosNovos.NomeLoja))
            {
                config.NomeLoja = dadosNovos.NomeLoja;
            }

            if (!string.IsNullOrEmpty(dadosNovos.Whatsapp))
                config.Whatsapp = dadosNovos.Whatsapp;

            if (!string.IsNullOrEmpty(dadosNovos.SenhaAdmin))
            {
                config.SenhaAdmin = dadosNovos.SenhaAdmin;
            }

            _context.SaveChanges();

            return Ok(config);
        }

        [HttpPost("login")]
        public IActionResult VerificarSenha([FromBody] string senhaDigitada) // <--- Recebe string simples
        {
            var config = _context.Configuracoes.FirstOrDefault();

            // CUIDADO: Se mandar JSON { "senha": "..." }, precisa criar um DTO ou usar dynamic.
            // Para simplificar, vamos assumir que o front manda a string crua com Content-Type application/json

            if (config != null && config.SenhaAdmin == senhaDigitada)
            {
                return Ok(new { mensagem = "Acesso Permitido" });
            }

            return Unauthorized(new { mensagem = "Senha Incorreta" });
        }
    }
}