namespace InspekcijskeKontrole.Web.Models;

public class InspekcijskaKontrola
{
    public int Id { get; set; }

    public DateTime DatumKontrole { get; set; }

    public int InspekcijskoTijeloId { get; set; }

    public InspekcijskoTijelo InspekcijskoTijelo { get; set; } = null!;

    public int ProizvodId { get; set; }

    public Proizvod Proizvod { get; set; } = null!;

    public string RezultatiKontrole { get; set; } = string.Empty;

    public bool ProizvodSiguran { get; set; }
}
