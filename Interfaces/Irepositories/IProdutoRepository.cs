using StudioTattooManagement.Models;

namespace StudioTattooManagement.Interfaces.Irepositories
{
    public interface IProdutoRepository
    {
        IEnumerable<Produto> ObterTodos();
        Produto ObterPorId(int id);
        void Adicionar(Produto produto);
        void Atualizar(Produto produto);
        void Remover(Produto produto);
        void SaveChanges();
    }
}
