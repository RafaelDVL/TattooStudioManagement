using StudioTattooManagement.Utils.UtilsClasses;
using System;
using System.ComponentModel.DataAnnotations;

namespace StudioTattooManagement.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int fornecedor_codigoId { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public Preco Preco { get; set; }

        public int? QuantidadeEmEstoque { get; private set; }

        public DateTime DataUltimaAtualizacao { get; private set; }

        [Required]
        public string Categoria { get; set; }

        public string? CodigoDeBarras { get; set; }

        [Required]
        public int EstoqueMinimo { get; set; }

        // Lista de URLs ou paths das imagens do produto
        public List<string> Fotos { get; private set; }

        
        public virtual Fornecedor? Fornecedor { get; set; }

        public Produto()
        {
            Fotos = new List<string>();
        }

        public Produto(
            string nome,
            string descricao,
            Preco preco,
            int quantidadeInicial,
            string categoria,
            string codigoDeBarras,
            int fornecedorCodigoId,
            int estoqueMinimo) : this()
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

        public void AdicionarFoto(string fotoUrl)
        {
            if (Fotos.Count >= 5)
                throw new InvalidOperationException("O produto não pode ter mais de 5 fotos.");

            Fotos.Add(fotoUrl);
            DataUltimaAtualizacao = DateTime.UtcNow;
        }

        public void RemoverFoto(string fotoUrl)
        {
            Fotos.Remove(fotoUrl);
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

            if (QuantidadeEmEstoque < EstoqueMinimo)
            {
                Console.WriteLine($"Alerta: O estoque do produto '{Nome}' está abaixo do mínimo de {EstoqueMinimo} unidades.");
            }
        }
    }
}
