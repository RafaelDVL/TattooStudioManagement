using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudioTattooManagement.Interfaces.Iservices;
using StudioTattooManagement.Models;
using StudioTattooManagement.Services;
using StudioTattooManagement.Utils.UtilsClasses;
using System;

namespace StudioTattooManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly IServiceBase<Produto> _produtoBase;
        private readonly IWebHostEnvironment _environment;

        public ProdutoController(IProdutoService produtoService, IWebHostEnvironment environment, IServiceBase<Produto> serviceBase)
        {
            _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _produtoBase = serviceBase ?? throw new ArgumentNullException(nameof(serviceBase));

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

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                var form = Request.Form;

                // Extraindo os campos do FormData
                var nome = form["nome"].ToString();
                var descricao = form["descricao"].ToString();

                // Convertendo o preço de string para decimal e criando o objeto Preco
                if (!decimal.TryParse(form["preco"], out var precoDecimal))
                {
                    return BadRequest("Preço inválido.");
                }
                var preco = Preco.Criar(precoDecimal);

                // Convertendo os campos numéricos
                if (!int.TryParse(form["quantidadeEmEstoque"], out var quantidadeEmEstoque))
                {
                    return BadRequest("Quantidade em estoque inválida.");
                }

                if (!int.TryParse(form["fornecedor_codigoId"], out var fornecedorCodigoId))
                {
                    return BadRequest("Código do fornecedor inválido.");
                }

                if (!int.TryParse(form["estoqueMinimo"], out var estoqueMinimo))
                {
                    return BadRequest("Estoque mínimo inválido.");
                }

                var categoria = form["categoria"].ToString();
                var codigoDeBarras = form["codigoDeBarras"].ToString();

                // Verificar se o nome do produto já existe
                var nomeExistente = await _produtoBase.FindAsync(f => f.Nome == nome);
                if (nomeExistente.Any())
                {
                    return BadRequest("Já existe um produto com esse nome.");
                }

                // Criando o novo produto usando o construtor
                var produto = new Produto(
                    nome: nome,
                    descricao: descricao,
                    preco: preco,
                    quantidadeInicial: quantidadeEmEstoque,
                    categoria: categoria,
                    codigoDeBarras: codigoDeBarras,
                    fornecedorCodigoId: fornecedorCodigoId,
                    estoqueMinimo: estoqueMinimo
                );

                var imagem = form.Files.GetFile("imagem");
                if (imagem != null && imagem.Length > 0)
                {
                    // Obtém o caminho absoluto da pasta "Arquivos" dentro do projeto
                    var imageDirectory = Path.Combine(_environment.ContentRootPath, "Arquivos");

                    // Verifica se o diretório de imagens existe, senão cria.
                    if (!Directory.Exists(imageDirectory))
                        Directory.CreateDirectory(imageDirectory);

                    // Extrai a extensão original do arquivo de imagem
                    var fileExtension = Path.GetExtension(imagem.FileName);

                    // Define o nome da imagem com base no nome do fornecedor
                    var fileName = $"{nome.Replace(" ", "")}{fileExtension}";

                    // Define o caminho completo para salvar a imagem
                    var filePath = Path.Combine(imageDirectory, fileName);

                    // Salva o arquivo no sistema
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagem.CopyToAsync(stream);
                    }

                    // Salva o caminho relativo da imagem no fornecedor
                    produto.ImagemUrl = $"/Arquivos/{fileName}";  // Caminho relativo
                }

                // Adiciona o fornecedor ao banco de dados
                await _produtoBase.AddAsync(produto);
                await _produtoBase.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = produto.Id }, produto);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Ocorreu um erro ao salvar o fornecedor. Tente novamente mais tarde.");
            }
            catch (IOException)
            {
                return StatusCode(500, "Erro ao salvar a imagem. Verifique as permissões do diretório.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro inesperado: {ex.Message}");
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
