using Aula11.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aula11.Repositories.Mappings
{
    public class CategoriaMap : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("TbCategoria");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("CatId")
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Nome).HasColumnName("CatNome").IsRequired();
        }
    }
}