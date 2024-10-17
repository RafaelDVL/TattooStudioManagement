using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudioTattooManagement.Models;

namespace StudioTattooManagement.Data.Configurations
{
    public class PedidoCompraProdutoConfiguration : IEntityTypeConfiguration<PedidoCompraProduto>
    {
        public void Configure(EntityTypeBuilder<PedidoCompraProduto> builder)
    {
        // Definir a chave composta (PedidoCompraId, ProdutoId)
        builder.HasKey(pcp => new { pcp.PedidoCompraId, pcp.ProdutoId });

        // Configurar a relação com PedidoCompra (muitos para um)
        builder.HasOne(pcp => pcp.PedidoCompra)
               .WithMany(pc => pc.PedidoCompraProduto)
               .HasForeignKey(pcp => pcp.PedidoCompraId);

        // Configurar a relação com Produto (muitos para um)
        builder.HasOne(pcp => pcp.Produto)
               .WithMany(p => p.PedidoCompraProduto)
               .HasForeignKey(pcp => pcp.ProdutoId);
    }
    }
}
