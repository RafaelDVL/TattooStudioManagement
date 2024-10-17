namespace StudioTattooManagement.Models
{
    public class PedidoCompraProduto
    {
        public int PedidoCompraId { get; set; }
        public int ProdutoId { get; set; }

        public PedidoCompra PedidoCompra{ get; set; }
        public Produto Produto { get; set; }

        public int Quantidade { get; set; }
    }
}
