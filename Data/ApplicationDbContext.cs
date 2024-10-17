using Microsoft.EntityFrameworkCore;
using StudioTattooManagement.Data.Configurations;
using StudioTattooManagement.Models;

namespace StudioTattooManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Defina os DbSets para cada uma das suas entidades

        public DbSet<Produto> Produto { get; set; }

        // Se precisar configurar as entidades, sobreponha o método OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
            modelBuilder.ApplyConfiguration(new FornecedorConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoCompraProdutoConfiguration());
        }
    }
}
