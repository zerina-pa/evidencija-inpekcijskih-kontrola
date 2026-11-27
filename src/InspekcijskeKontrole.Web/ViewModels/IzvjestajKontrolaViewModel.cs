using InspekcijskeKontrole.Web.Models;

namespace InspekcijskeKontrole.Web.ViewModels;

public class IzvjestajKontrolaViewModel
{
    public InspekcijskoTijelo Tijelo { get; set; } = null!;

    public DateOnly DatumOd { get; set; }

    public DateOnly DatumDo { get; set; }

    public DateTime DatumGenerisanja { get; set; }

    public IReadOnlyList<InspekcijskaKontrola> Kontrole { get; set; } =
        Array.Empty<InspekcijskaKontrola>();

    public int UkupanBrojKontrola => Kontrole.Count;
}
