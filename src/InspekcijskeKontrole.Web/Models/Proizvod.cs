namespace InspekcijskeKontrole.Web.Models;

public class Proizvod
{
    private string? _serijskiBroj;

    public int Id { get; set; }

    public string Naziv { get; set; } = string.Empty;

    public string Proizvodjac { get; set; } = string.Empty;

    public string? SerijskiBroj
    {
        get => _serijskiBroj;
        set => _serijskiBroj = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    public string ZemljaPorijekla { get; set; } = string.Empty;

    public string? Opis { get; set; }

    public ICollection<InspekcijskaKontrola> Kontrole { get; set; } = new List<InspekcijskaKontrola>();
}
