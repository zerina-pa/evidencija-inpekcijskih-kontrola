using InspekcijskeKontrole.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InspekcijskeKontrole.Web.Data.Configurations;

public class InspekcijskoTijeloConfiguration : IEntityTypeConfiguration<InspekcijskoTijelo>
{
    public void Configure(EntityTypeBuilder<InspekcijskoTijelo> builder)
    {
        builder.ToTable("InspekcijskaTijela");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Naziv)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Inspektorat)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.Nadleznost)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.KontaktOsoba)
            .IsRequired()
            .HasMaxLength(200);
    }
}
