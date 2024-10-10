using Microsoft.EntityFrameworkCore;
using StudioTattooManagement.Data;
using StudioTattooManagement.Interfaces.Irepositories;
using StudioTattooManagement.Models;

namespace StudioTattooManagement.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProdutoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Produto> ObterTodos()
        {
            return _context.Produto.AsNoTracking().ToList();
        }

        public Produto ObterPorId(int id)
        {
            return _context.Produto.Find(id);
        }

        public void Adicionar(Produto produto)
        {
            _context.Produto.Add(produto);
        }

        public void Atualizar(Produto produto)
        {
            _context.Produto.Update(produto);
        }

        public void Remover(Produto produto)
        {
            _context.Produto.Remove(produto);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
