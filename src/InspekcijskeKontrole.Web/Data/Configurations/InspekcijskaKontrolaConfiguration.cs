using InspekcijskeKontrole.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InspekcijskeKontrole.Web.Data.Configurations;

public class InspekcijskaKontrolaConfiguration : IEntityTypeConfiguration<InspekcijskaKontrola>
{
    public void Configure(EntityTypeBuilder<InspekcijskaKontrola> builder)
    {
        builder.ToTable("InspekcijskeKontrole");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DatumKontrole)
            .IsRequired();

        builder.Property(x => x.RezultatiKontrole)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.ProizvodSiguran)
            .IsRequired();

        builder.HasOne(x => x.InspekcijskoTijelo)
            .WithMany(x => x.Kontrole)
            .HasForeignKey(x => x.InspekcijskoTijeloId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Proizvod)
            .WithMany(x => x.Kontrole)
            .HasForeignKey(x => x.ProizvodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.InspekcijskoTijeloId, x.DatumKontrole })
            .HasDatabaseName("IX_InspekcijskaKontrola_Tijelo_Datum");
    }
}
