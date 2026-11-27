using InspekcijskeKontrole.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InspekcijskeKontrole.Web.Data.Configurations;

public class ProizvodConfiguration : IEntityTypeConfiguration<Proizvod>
{
    public void Configure(EntityTypeBuilder<Proizvod> builder)
    {
        builder.ToTable("Proizvodi");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Naziv)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Proizvodjac)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.SerijskiBroj)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(x => x.ZemljaPorijekla)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Opis)
            .IsRequired(false)
            .HasMaxLength(2000);

        builder.HasIndex(x => x.SerijskiBroj)
            .IsUnique()
            .HasDatabaseName("UX_Proizvod_SerijskiBroj")
            .HasFilter("[SerijskiBroj] IS NOT NULL");

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Proizvod_SerijskiBroj",
            "[SerijskiBroj] IS NULL OR length(trim([SerijskiBroj])) > 0"));
    }
}
