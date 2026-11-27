namespace InspekcijskeKontrole.Web.Models;

public class InspekcijskoTijelo
{
    public int Id { get; set; }

    public string Naziv { get; set; } = string.Empty;

    public Inspektorat Inspektorat { get; set; }

    public Nadleznost Nadleznost { get; set; }

    public string KontaktOsoba { get; set; } = string.Empty;

    public ICollection<InspekcijskaKontrola> Kontrole { get; set; } = new List<InspekcijskaKontrola>();
}
