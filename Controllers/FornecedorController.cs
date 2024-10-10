using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudioTattooManagement.Interfaces.Iservices;
using StudioTattooManagement.Models;

namespace StudioTattooManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FornecedorController : ControllerBase
    {
        private readonly IServiceBase<Fornecedor> _fornecedorService;
        private readonly string _imageDirectory = @"C:\images";

        public FornecedorController(IServiceBase<Fornecedor> fornecedorService)
        {
            _fornecedorService = fornecedorService ?? throw new ArgumentNullException(nameof(fornecedorService));
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
        public async Task<IActionResult> Post([FromForm] Fornecedor fornecedor, IFormFile foto)
        {

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Verifica se há uma foto
                if (foto != null && foto.Length > 0)
                {
                    // Verifica se o diretório de imagens existe, senão cria.
                    if (!Directory.Exists(_imageDirectory))
                        Directory.CreateDirectory(_imageDirectory);

                    // Define um nome único para o arquivo de imagem
                    var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(foto.FileName)}";
                    var filePath = Path.Combine(_imageDirectory, fileName);

                    // Salva a imagem no diretório "C:\images"
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await foto.CopyToAsync(stream);
                    }

                    // Salva o caminho da imagem no fornecedor (salvando o caminho completo ou apenas o nome do arquivo)
                    fornecedor.ImagemUrl = filePath;
                }

                // Adiciona o fornecedor ao banco de dados
                await _fornecedorService.AddAsync(fornecedor);
                await _fornecedorService.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = fornecedor.Id }, fornecedor);
            }
            catch (DbUpdateException ex)
            {
                // Retornar um erro 500 com uma mensagem personalizada
                return StatusCode(500, "Ocorreu um erro ao salvar o fornecedor. Por favor, tente novamente mais tarde.");
            }
            catch (IOException ex)
            {
                // Retornar um erro 500 com uma mensagem personalizada
                return StatusCode(500, "Ocorreu um erro ao salvar a imagem do fornecedor. Por favor, verifique as permissões do diretório de imagens.");
            }
            catch (Exception ex)
            {
                // Retornar um erro 500 com uma mensagem genérica
                return StatusCode(500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] Fornecedor fornecedor, IFormFile foto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != fornecedor.Id)
                return BadRequest("O ID do fornecedor não corresponde ao ID fornecido na URL.");

            // Atualiza a imagem caso uma nova seja fornecida
            if (foto != null && foto.Length > 0)
            {
                if (!Directory.Exists(_imageDirectory))
                    Directory.CreateDirectory(_imageDirectory);

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(foto.FileName)}";
                var filePath = Path.Combine(_imageDirectory, fileName);

                // Salva a nova imagem
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }

                // Atualiza o caminho da imagem no fornecedor
                fornecedor.ImagemUrl = filePath;
            }

            _fornecedorService.Update(fornecedor);
            await _fornecedorService.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var fornecedor = await _fornecedorService.GetByIdAsync(id);
            if (fornecedor == null)
                return NotFound();

            // Deleta a imagem associada, se existir
            if (!string.IsNullOrEmpty(fornecedor.ImagemUrl) && System.IO.File.Exists(fornecedor.ImagemUrl))
            {
                System.IO.File.Delete(fornecedor.ImagemUrl);
            }

            _fornecedorService.Remove(fornecedor);
            await _fornecedorService.SaveChangesAsync();

            return NoContent();
        }
    }
}
