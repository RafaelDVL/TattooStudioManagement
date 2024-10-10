using StudioTattooManagement.Models;

namespace StudioTattooManagement.Interfaces.Iservices
{
    public interface IProdutoService
    {
        IEnumerable<Produto> ObterTodos();
        Produto ObterPorId(int id);
        void AdicionarProduto(Produto produto);
        void AtualizarProduto(Produto produto);
        void RemoverProduto(int id);
        void AdicionarEstoque(int produtoId, int quantidade);
        void RemoverEstoque(int produtoId, int quantidade);
    }
}
