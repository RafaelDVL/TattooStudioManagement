using StudioTattooManagement.Data;
using StudioTattooManagement.Interfaces.Iservices;
using StudioTattooManagement.Models;
using Microsoft.EntityFrameworkCore;
using StudioTattooManagement.Interfaces.Irepositories;
using StudioTattooManagement.Repositories;
using System.Collections.Generic;

namespace StudioTattooManagement.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository ?? throw new ArgumentNullException(nameof(produtoRepository));
        }

        public IEnumerable<Produto> ObterTodos()
        {
            return _produtoRepository.ObterTodos();
        }

        public Produto ObterPorId(int id)
        {
            var produto = _produtoRepository.ObterPorId(id);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com o ID {id} não foi encontrado.");
            return produto;
        }

        public void AdicionarProduto(Produto produto)
        {
            if (produto == null)
                throw new ArgumentNullException(nameof(produto));

            _produtoRepository.Adicionar(produto);
            _produtoRepository.SaveChanges();
        }

        public void AtualizarProduto(Produto produto)
        {
            if (produto == null)
                throw new ArgumentNullException(nameof(produto));

            var produtoExistente = _produtoRepository.ObterPorId(produto.Id);
            if (produtoExistente == null)
                throw new KeyNotFoundException($"Produto com o ID {produto.Id} não foi encontrado.");

            _produtoRepository.Atualizar(produto);
            _produtoRepository.SaveChanges();
        }

        public void RemoverProduto(int id)
        {
            var produto = _produtoRepository.ObterPorId(id);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com o ID {id} não foi encontrado.");

            _produtoRepository.Remover(produto);
            _produtoRepository.SaveChanges();
        }

        public void AdicionarEstoque(int produtoId, int quantidade)
        {
            var produto = ObterPorId(produtoId);
            produto.AdicionarEstoque(quantidade);
            _produtoRepository.SaveChanges();
        }

        public void RemoverEstoque(int produtoId, int quantidade)
        {
            var produto = ObterPorId(produtoId);
            produto.RemoverEstoque(quantidade);
            _produtoRepository.SaveChanges();
        }

        public void AdicionarFoto(int produtoId, string fotoUrl)
        {
            var produto = ObterPorId(produtoId);
            produto.AdicionarFoto(fotoUrl);
            _produtoRepository.SaveChanges();
        }

        public void RemoverFoto(int produtoId, string fotoUrl)
        {
            var produto = ObterPorId(produtoId);
            produto.RemoverFoto(fotoUrl);
            _produtoRepository.SaveChanges();
        }
    }
}
