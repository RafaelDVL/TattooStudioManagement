using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudioTattooManagement.Models;

namespace StudioTattooManagement.Data.Configurations
{
    public class FornecedorConfiguration: IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            // Configuração da tabela
            builder.ToTable("Fornecedores");

            // Configurações das propriedades
            builder.HasKey(f => f.Id);

            builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

            builder.Property(f => f.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.LinkUltimaCompra)
                .HasMaxLength(500); // URL pode ser longa, ajuste conforme necessário

            builder.Property(f => f.ImagemUrl)
                .HasMaxLength(500); // URL ou caminho para a imagem, ajuste conforme necessário

            // Relacionamento configurado no ProdutoConfiguration
        }
    }
}
