namespace InspekcijskeKontrole.Web.ViewModels;

public class ZapisnikKontroleViewModel
{
    public int Id { get; set; }

    public string SerijskiBroj { get; set; } = "–";

    public string NazivProizvoda { get; set; } = string.Empty;

    public string ZemljaPorijekla { get; set; } = string.Empty;

    public DateTime DatumKontrole { get; set; }

    public string RezultatiKontrole { get; set; } = string.Empty;

    public string NazivInspekcijskogTijela { get; set; } = string.Empty;

    public bool ProizvodSiguran { get; set; }
}
