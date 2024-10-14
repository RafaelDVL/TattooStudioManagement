namespace StudioTattooManagement.DTOs
{
    public class FornecedorDto
    {
        public string Nome { get; set; }
        public string? LinkUltimaCompra { get; set; }
        public string? ImagemUrl { get; set; } // Esse campo será atualizado com o nome do arquivo
    }
}
