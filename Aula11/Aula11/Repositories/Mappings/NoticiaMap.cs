using Aula11.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aula11.Repositories.Mappings
{
    public class NoticiaMap : IEntityTypeConfiguration<Noticia>
    {
        public void Configure(EntityTypeBuilder<Noticia> builder)
        {
            //Configurando qual tabela sera mapeada para a classe Noticia
            builder.ToTable("TbNoticia");

            //Indica qual objeto é a chave primária da tabela
            builder.HasKey(x => x.Id);

            //Property, serve para mapear os objetos com  os campos da tabela
            builder.Property(x => x.Id)
                .HasColumnName("NotId")
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Titulo)
                .HasColumnName("NotTitulo")
                //.HasColumnType("VARCHAR")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Texto)
                .HasColumnName("NotTexto")
                .HasColumnType("TEXT")
                .IsRequired();

            builder.Property(x => x.Data).HasColumnName("NotData").IsRequired();

            //builder.Property(x => x.Categoria).HasColumnName("CatId").IsRequired();
            builder
                .HasOne(x => x.Categoria)
                .WithMany(x => x.Noticias)
                .HasForeignKey("CatId")
                .IsRequired();
        }
    }
}