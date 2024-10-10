using Microsoft.AspNetCore.Mvc;
using StudioTattooManagement.Interfaces.Iservices;
using StudioTattooManagement.Models;

namespace StudioTattooManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutoController(IProdutoService produtoService)
        {
            _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
        }

        // GET: api/produto
        [HttpGet]
        public IActionResult Get()
        {
            var produtos = _produtoService.ObterTodos();
            return Ok(produtos);
        }

        // GET: api/produto/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var produto = _produtoService.ObterPorId(id);
                return Ok(produto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // POST: api/produto
        [HttpPost]
        public IActionResult Post([FromBody] Produto produto)
        {

            try
            {
                // Adicionar o produto usando o serviço
                _produtoService.AdicionarProduto(produto);

                // Retornar a resposta com CreatedAtAction para incluir o recurso recém-criado
                return CreatedAtAction(
                    nameof(Get),
                    new { id = produto.Id },
                    produto);
            }
            catch (ArgumentException ex)
            {
                // Captura ArgumentException que pode incluir ArgumentNullException e outras
                return BadRequest(new { Message = "Erro de validação nos dados do produto.", Detalhe = ex.Message });
            }
            catch (Exception ex)
            {
                // Captura erros inesperados e retorna uma resposta de erro genérica
                return StatusCode(500, new { Message = "Ocorreu um erro ao processar a solicitação.", Detalhe = ex.Message });
            }
        }

        // PUT: api/produto/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Produto produto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != produto.Id)
                return BadRequest(new { Message = "O ID do produto não corresponde ao ID fornecido na URL." });

            try
            {
                _produtoService.AtualizarProduto(produto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/produto/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _produtoService.RemoverProduto(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // PATCH: api/produto/{id}/adicionar-estoque/{quantidade}
        [HttpPatch("{id}/adicionar-estoque/{quantidade}")]
        public IActionResult AdicionarEstoque(int id, int quantidade)
        {
            try
            {
                _produtoService.AdicionarEstoque(id, quantidade);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PATCH: api/produto/{id}/remover-estoque/{quantidade}
        [HttpPatch("{id}/remover-estoque/{quantidade}")]
        public IActionResult RemoverEstoque(int id, int quantidade)
        {
            try
            {
                _produtoService.RemoverEstoque(id, quantidade);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
