using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StudioTattooManagement.Models;

namespace StudioTattooManagement.Data.Configurations
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            // Configuração da tabela
            builder.ToTable("Produtos");

            // Configurações das propriedades
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Descricao)
                .HasMaxLength(500);

            builder.Property(p => p.Categoria)
                .HasMaxLength(50);

            builder.Property(p => p.CodigoDeBarras)
                .HasMaxLength(20);

            builder.Property(p => p.QuantidadeEmEstoque);

            builder.Property(p => p.EstoqueMinimo)
                .IsRequired();

            // Configuração do preço usando um Value Object
            builder.OwnsOne(p => p.Preco, preco =>
            {
                preco.Property(p => p.Valor)
                    .HasColumnName("Preco")
                    .IsRequired()
                    .HasPrecision(18, 2); // Define precisão e escala para valores monetários
            });

            // Configuração do relacionamento com Fornecedor
            builder.HasOne(p => p.Fornecedor)
                .WithMany(f => f.Produtos)
                .HasForeignKey(p => p.fornecedor_codigoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
