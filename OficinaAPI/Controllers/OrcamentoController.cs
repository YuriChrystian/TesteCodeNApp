using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OficinaAPI.Models;

namespace OficinaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrcamentoController : ControllerBase
    {
        private static List<OrcamentoModel> _orcamentos = new List<OrcamentoModel>();

        [HttpPost("CriarOrcamento")]
        public IActionResult CriarOrcamento([FromBody] OrcamentoModel novoOrcamento)
        {
            if (novoOrcamento.Itens == null || !novoOrcamento.Itens.Any())
            {
                return BadRequest("O Orçamento deve conter pelo menos um item");
            }
            foreach (var item in novoOrcamento.Itens)
            {
                if (item.Quantidade <= 0)
                {
                    return BadRequest($"O item '{item.Descricao}' deve ter quantidade maior que zero.");
                }
                if (item.ValorUnitario <= 0)
                {
                    return BadRequest($"O item '{item.Descricao}' deve ter um valor unitário maior que zero.");
                }
            }

            _orcamentos.Add(novoOrcamento);

            var resultado = new
            {
                Mensagem = "Orçamento cadastrado com sucesso!",
                TotalGeral = novoOrcamento.ValorTotal,
                Dados = novoOrcamento
            };

            return Ok(new { Mensagem = "Criação realizada com sucesso", Dados = novoOrcamento });
        }

        [HttpGet("ListarOrcamentos")]
        public IActionResult ListarOrcamentos()
        {
            return Ok(_orcamentos);
        }
    }
}
