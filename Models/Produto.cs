using StudioTattooManagement.Utils.UtilsClasses;
using System;

namespace StudioTattooManagement.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public int fornecedor_codigoId { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public Preco Preco { get; set; }

        public int QuantidadeEmEstoque { get; private set; }

        public DateTime DataUltimaAtualizacao { get; private set; }

        public string Categoria { get; set; }

        public string CodigoDeBarras { get; set; }

        public Fornecedor Fornecedor { get; set; }

        public int EstoqueMinimo { get; set; }

        public Produto() { }

        public Produto(
    string nome,
    string descricao,
    Preco preco,
    int quantidadeInicial,
    string categoria,
    string codigoDeBarras,
    int fornecedorCodigoId,
    int estoqueMinimo)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do produto não pode ser vazio.", nameof(nome));

            if (quantidadeInicial < 0)
                throw new ArgumentException("A quantidade inicial em estoque não pode ser negativa.", nameof(quantidadeInicial));

            if (estoqueMinimo < 0)
                throw new ArgumentException("O estoque mínimo não pode ser negativo.", nameof(estoqueMinimo));

            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            QuantidadeEmEstoque = quantidadeInicial;
            Categoria = categoria;
            CodigoDeBarras = codigoDeBarras;
            fornecedor_codigoId = fornecedorCodigoId;
            EstoqueMinimo = estoqueMinimo;
            DataUltimaAtualizacao = DateTime.UtcNow;
        }

        public void AdicionarEstoque(int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentException("A quantidade a ser adicionada deve ser maior que zero.", nameof(quantidade));

            QuantidadeEmEstoque += quantidade;
            DataUltimaAtualizacao = DateTime.UtcNow;
        }

        public void RemoverEstoque(int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentException("A quantidade a ser removida deve ser maior que zero.", nameof(quantidade));

            if (quantidade > QuantidadeEmEstoque)
                throw new InvalidOperationException("A quantidade a ser removida excede a quantidade disponível no estoque.");

            QuantidadeEmEstoque -= quantidade;
            DataUltimaAtualizacao = DateTime.UtcNow;

            // Verificar se o estoque está abaixo do mínimo e disparar alerta (exemplo)
            if (QuantidadeEmEstoque < EstoqueMinimo)
            {
                Console.WriteLine($"Alerta: O estoque do produto '{Nome}' está abaixo do mínimo de {EstoqueMinimo} unidades.");
            }
        }
    }
}
