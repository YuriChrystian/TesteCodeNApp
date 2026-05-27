using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OficinaAPI.Data;
using OficinaAPI.Models;

namespace OficinaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrcamentoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrcamentoController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost("CriarOrcamento")]
        public async Task<IActionResult> CriarOrcamento([FromBody] OrcamentoModel novoOrcamento)
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

            try 
            {
                await _context.Orcamentos.AddAsync(novoOrcamento);
                await _context.SaveChangesAsync();

                var resultado = new
                {
                    Mensagem = "Orçamento cadastrado com sucesso!",
                    TotalGeral = novoOrcamento.ValorTotal,
                    Dados = novoOrcamento
                };

                return CreatedAtAction(nameof(ListarOrcamentos), new { id = novoOrcamento.Id }, resultado);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Mensagem = "Erro ao salvar o orçamento no banco de dados.", Detalhe = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Mensagem = "Ocorreu um erro interno no servidor.", Detalhe = ex.Message });
            }
        }

        [HttpGet("ListarOrcamentos")]
        public async Task<IActionResult> ListarOrcamentos()
        {
            try
            {
                var lista = await _context.Orcamentos.Include(o => o.Itens).ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Mensagem = "Erro ao recuperar os orçamentos.", Detalhe = ex.Message });
            }
        }

        [HttpDelete("ExcluirOrcamento/{id}")]
        public async Task<IActionResult> ExcluirOrcamento(int id)
        {
            try
            {
                var orcamento = await _context.Orcamentos.FirstOrDefaultAsync(o => o.Id == id);
                if (orcamento == null)
                {
                    return NotFound("Orçamento não encontrado");
                }
                _context.Orcamentos.Remove(orcamento);
                await _context.SaveChangesAsync();

                return Ok(new { Mensagem = "Orçamento excluído com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Mensagem = "Erro ao excluir o orçamento.", Detalhe = ex.Message });
            }
        }

        [HttpPut("AtualizarOrcamento/{id}")]
        public async Task<IActionResult> AtualizarOrcamento(int id, [FromBody] OrcamentoModel orcamentoAtualizado)
        {
            if (orcamentoAtualizado.Itens == null || !orcamentoAtualizado.Itens.Any())
            {
                return BadRequest("O Orçamento deve conter pelo menos um item");
            }

            foreach (var item in orcamentoAtualizado.Itens)
            {
                if (item.Quantidade <= 0 || item.ValorUnitario <= 0)
                {
                    return BadRequest($"O item '{item.Descricao}' deve ter quantidade e valor maiores que zero.");
                }
            }
            try
            {
                var orcamento = await _context.Orcamentos.FirstOrDefaultAsync(o => o.Id == id);

                if (orcamento == null)
                {
                    return NotFound("Orçamento não encontrado");
                }

                if (orcamentoAtualizado.ClienteId > 0) orcamento.ClienteId = orcamentoAtualizado.ClienteId;
                if (orcamentoAtualizado.VeiculoId > 0) orcamento.VeiculoId = orcamentoAtualizado.VeiculoId;

                _context.RemoveRange(orcamento.Itens);
                await _context.SaveChangesAsync();
                orcamento.Itens = orcamentoAtualizado.Itens;
                await _context.SaveChangesAsync();

                return Ok(new { Mensagem = "Orçamento atualizado com sucesso", Dados = orcamento });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Mensagem = "Erro ao atualizar o orçamento.", Detalhe = ex.Message });
            }
        }
    }
}

