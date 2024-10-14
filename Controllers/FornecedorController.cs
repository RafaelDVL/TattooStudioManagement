using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudioTattooManagement.DTOs;
using StudioTattooManagement.Interfaces.Iservices;
using StudioTattooManagement.Models;
using System;

namespace StudioTattooManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FornecedorController : ControllerBase
    {
        private readonly IServiceBase<Fornecedor> _fornecedorService;
        private readonly IWebHostEnvironment _environment;

        public FornecedorController(IServiceBase<Fornecedor> fornecedorService, IWebHostEnvironment environment)
        {
            _fornecedorService = fornecedorService ?? throw new ArgumentNullException(nameof(fornecedorService));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fornecedores = await _fornecedorService.GetAllAsync();
            return Ok(fornecedores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var fornecedor = await _fornecedorService.GetByIdAsync(id);
            if (fornecedor == null)
                return NotFound();
            return Ok(fornecedor);
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                var form = Request.Form;

                // Extraindo os campos de texto do FormData
                var nome = form["nome"].ToString();

                var nomeExistente = await _fornecedorService.FindAsync(f => f.Nome == nome);

                if (nomeExistente.Any())
                {
                    // Se o nome já existir, você pode retornar uma resposta apropriada
                    return BadRequest("Já existe um fornecedor com esse nome.");
                }

                var linkUltimaCompra = form["linkUltimaCompra"].ToString();

                // Cria um objeto Fornecedor com os dados extraídos do FormData
                var fornecedor = new Fornecedor
                {
                    Nome = nome,
                    LinkUltimaCompra = linkUltimaCompra
                };

                // Extraindo o arquivo de imagem (se presente)
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
                    fornecedor.ImagemUrl = $"/Arquivos/{fileName}";  // Caminho relativo
                }

                // Adiciona o fornecedor ao banco de dados
                await _fornecedorService.AddAsync(fornecedor);
                await _fornecedorService.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = fornecedor.Id }, fornecedor);
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


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id)
        {
            try
            {
                // Extraindo os campos de texto do FormData
                var form = Request.Form;

                var nome = form["nome"].ToString();
                var linkUltimaCompra = form["linkUltimaCompra"].ToString();
                var imagemUrl = form["imagemUrl"].ToString();
                var imagem = form.Files.GetFile("imagem");

                var fornecedorExistente = await _fornecedorService.GetByIdAsync(id);
                if (fornecedorExistente == null)
                    return NotFound("Fornecedor não encontrado.");

                // Atualiza a imagem caso uma nova seja fornecida
                if (imagem != null && imagem.Length > 0)
                {
                    var imageDirectory = Path.Combine(_environment.ContentRootPath, "Arquivos");

                    if (!Directory.Exists(imageDirectory))
                        Directory.CreateDirectory(imageDirectory);

                    var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(imagem.FileName)}";
                    var filePath = Path.Combine(imageDirectory, fileName);

                    // Salva a nova imagem
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagem.CopyToAsync(stream);
                    }

                    // Atualiza o caminho da imagem no fornecedor
                    imagemUrl = $"/Arquivos/{fileName}";
                }


                fornecedorExistente.Nome = nome;
                fornecedorExistente.LinkUltimaCompra = linkUltimaCompra;
                fornecedorExistente.ImagemUrl = imagemUrl;

                _fornecedorService.Update(fornecedorExistente);
                await _fornecedorService.SaveChangesAsync();

                return NoContent();
            }
            catch (DirectoryNotFoundException dirEx)
            {
                // Tratamento de erro específico para problemas com diretórios
                return StatusCode(500, $"Erro ao acessar o diretório: {dirEx.Message}");
            }
            catch (IOException ioEx)
            {
                // Tratamento de erro para problemas de I/O ao salvar a imagem
                return StatusCode(500, $"Erro de I/O ao salvar a imagem: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                // Captura de qualquer outro tipo de exceção
                return StatusCode(500, $"Ocorreu um erro ao atualizar o fornecedor: {ex.Message}");
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var fornecedor = await _fornecedorService.GetByIdAsync(id);
            if (fornecedor == null)
                return NotFound();

            // Deleta a imagem associada, se existir
            var imageDirectory = Path.Combine(_environment.ContentRootPath, "Arquivos");
            var filePath = Path.Combine(imageDirectory, fornecedor.ImagemUrl);

            if (!string.IsNullOrEmpty(fornecedor.ImagemUrl) && System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _fornecedorService.Remove(fornecedor);
            await _fornecedorService.SaveChangesAsync();

            return NoContent();
        }
    }

}
