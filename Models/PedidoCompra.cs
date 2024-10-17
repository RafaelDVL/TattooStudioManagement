namespace StudioTattooManagement.Models
{
    public class PedidoCompra
    {
        public int PedidoCompraId { get; set; }
        public string Nome_Cliente { get; set; }

        public DateTime DataCriacao { get; set; } 


        public List<PedidoCompraProduto> PedidoCompraProduto { get; set; }
    }
}
