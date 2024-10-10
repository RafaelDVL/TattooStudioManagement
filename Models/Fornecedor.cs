using System.ComponentModel.DataAnnotations;

namespace StudioTattooManagement.Models
{
    public class Fornecedor
    {
        public int Id { get; set; }

        [Required]
        public required string Nome { get; set; }

        public string? LinkUltimaCompra { get; set; }

        public string? ImagemUrl { get; set; }

        public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }


}
