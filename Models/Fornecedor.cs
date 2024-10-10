namespace StudioTattooManagement.Models
{
    public class Fornecedor
    {
        public int Id { get; set; }

        // Nome do fornecedor (ex.: "WJX", "Mercado Livre", etc.)
        public string Nome { get; set; }

        // Link do último local onde a compra foi feita (ex.: URL de um site)
        public string LinkUltimaCompra { get; set; }

        // Caminho para a imagem do fornecedor (URL online ou caminho de diretório local)
        public string ImagemUrl { get; set; }

        // Lista de produtos fornecidos por este fornecedor
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }


}
