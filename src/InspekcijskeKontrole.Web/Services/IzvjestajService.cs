using InspekcijskeKontrole.Web.Data;
using InspekcijskeKontrole.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InspekcijskeKontrole.Web.Services;

public class IzvjestajService : IIzvjestajService
{
    private static readonly TimeZoneInfo SarajevoVremenskaZona =
        TimeZoneInfo.FindSystemTimeZoneById("Europe/Sarajevo");

    private readonly ApplicationDbContext _context;
    private readonly TimeProvider _timeProvider;

    public IzvjestajService(ApplicationDbContext context, TimeProvider timeProvider)
    {
        _context = context;
        _timeProvider = timeProvider;
    }

    public async Task<IzvjestajRezultat<IzvjestajKontrolaViewModel>>
        DohvatiKontrolePoTijeluZaPeriodAsync(
            int inspekcijskoTijeloId,
            DateOnly datumOd,
            DateOnly datumDo)
    {
        if (datumOd > datumDo)
        {
            return IzvjestajRezultat<IzvjestajKontrolaViewModel>.Neuspjesan(
                "Datum od ne može biti poslije datuma do.",
                nameof(IzvjestajKontrolaParametriViewModel.DatumOd));
        }

        var tijelo = await _context.InspekcijskaTijela
            .FirstOrDefaultAsync(x => x.Id == inspekcijskoTijeloId);

        if (tijelo is null)
        {
            return IzvjestajRezultat<IzvjestajKontrolaViewModel>.Neuspjesan(
                "Izabrano inspekcijsko tijelo ne postoji.",
                nameof(inspekcijskoTijeloId));
        }

        var pocetak = datumOd.ToDateTime(TimeOnly.MinValue);
        var kraj = datumDo.AddDays(1).ToDateTime(TimeOnly.MinValue);

        var kontrole = await _context.InspekcijskeKontrole
            .Include(x => x.Proizvod)
            .Include(x => x.InspekcijskoTijelo)
            .Where(x =>
                x.InspekcijskoTijeloId == inspekcijskoTijeloId &&
                x.DatumKontrole >= pocetak &&
                x.DatumKontrole < kraj)
            .OrderBy(x => x.DatumKontrole)
            .ToListAsync();

        var trenutno = TimeZoneInfo.ConvertTime(
            _timeProvider.GetUtcNow(),
            SarajevoVremenskaZona);

        var datumGenerisanja = new DateTime(
            trenutno.Year,
            trenutno.Month,
            trenutno.Day,
            trenutno.Hour,
            trenutno.Minute,
            trenutno.Second,
            DateTimeKind.Unspecified);

        var izvjestaj = new IzvjestajKontrolaViewModel
        {
            Tijelo = tijelo,
            DatumOd = datumOd,
            DatumDo = datumDo,
            DatumGenerisanja = datumGenerisanja,
            Kontrole = kontrole
        };

        return IzvjestajRezultat<IzvjestajKontrolaViewModel>.Uspjesan(izvjestaj);
    }

    public async Task<ZapisnikKontroleViewModel?> DohvatiZapisnikPoIdAsync(int id)
    {
        var kontrola = await _context.InspekcijskeKontrole
            .Include(x => x.InspekcijskoTijelo)
            .Include(x => x.Proizvod)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (kontrola is null)
        {
            return null;
        }

        return new ZapisnikKontroleViewModel
        {
            Id = kontrola.Id,
            SerijskiBroj = kontrola.Proizvod.SerijskiBroj ?? "–",
            NazivProizvoda = kontrola.Proizvod.Naziv,
            ZemljaPorijekla = kontrola.Proizvod.ZemljaPorijekla,
            DatumKontrole = kontrola.DatumKontrole,
            RezultatiKontrole = kontrola.RezultatiKontrole,
            NazivInspekcijskogTijela = kontrola.InspekcijskoTijelo.Naziv,
            ProizvodSiguran = kontrola.ProizvodSiguran
        };
    }
}
