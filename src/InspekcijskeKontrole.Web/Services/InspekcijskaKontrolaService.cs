using InspekcijskeKontrole.Web.Data;
using InspekcijskeKontrole.Web.Models;
using InspekcijskeKontrole.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InspekcijskeKontrole.Web.Services;

public class InspekcijskaKontrolaService : IInspekcijskaKontrolaService
{
    private static readonly TimeZoneInfo SarajevoVremenskaZona =
        TimeZoneInfo.FindSystemTimeZoneById("Europe/Sarajevo");

    private readonly ApplicationDbContext _context;
    private readonly TimeProvider _timeProvider;

    public InspekcijskaKontrolaService(
        ApplicationDbContext context,
        TimeProvider timeProvider)
    {
        _context = context;
        _timeProvider = timeProvider;
    }

    public async Task<IReadOnlyList<InspekcijskaKontrola>> DohvatiListuAsync()
    {
        return await _context.InspekcijskeKontrole
            .Include(x => x.InspekcijskoTijelo)
            .Include(x => x.Proizvod)
            .OrderByDescending(x => x.DatumKontrole)
            .ToListAsync();
    }

    public Task<InspekcijskaKontrola?> DohvatiPoIdAsync(int id) =>
        _context.InspekcijskeKontrole
            .Include(x => x.InspekcijskoTijelo)
            .Include(x => x.Proizvod)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<InspekcijskaKontrolaRezultat<InspekcijskaKontrola>> KreirajAsync(
        InspekcijskaKontrolaFormModel model)
    {
        var datumRezultat = ProvjeriDatum(model.DatumKontrole);
        if (datumRezultat is not null)
        {
            return datumRezultat;
        }

        var tijeloRezultat = await ProvjeriTijeloIProizvodAsync(model);
        if (tijeloRezultat is not null)
        {
            return tijeloRezultat;
        }

        var kontrola = new InspekcijskaKontrola
        {
            DatumKontrole = model.DatumKontrole!.Value,
            InspekcijskoTijeloId = model.InspekcijskoTijeloId,
            ProizvodId = model.ProizvodId,
            RezultatiKontrole = model.RezultatiKontrole.Trim(),
            ProizvodSiguran = model.ProizvodSiguran
        };

        _context.InspekcijskeKontrole.Add(kontrola);
        await _context.SaveChangesAsync();

        return InspekcijskaKontrolaRezultat<InspekcijskaKontrola>.Uspjesan(kontrola);
    }

    public async Task<InspekcijskaKontrolaRezultat<InspekcijskaKontrola>> IzmijeniAsync(
        int id,
        InspekcijskaKontrolaFormModel model)
    {
        var datumRezultat = ProvjeriDatum(model.DatumKontrole);
        if (datumRezultat is not null)
        {
            return datumRezultat;
        }

        var tijeloRezultat = await ProvjeriTijeloIProizvodAsync(model);
        if (tijeloRezultat is not null)
        {
            return tijeloRezultat;
        }

        var kontrola = await _context.InspekcijskeKontrole.FindAsync(id);
        if (kontrola is null)
        {
            return InspekcijskaKontrolaRezultat<InspekcijskaKontrola>.Neuspjesan(
                "Inspekcijska kontrola nije pronađena.");
        }

        kontrola.DatumKontrole = model.DatumKontrole!.Value;
        kontrola.InspekcijskoTijeloId = model.InspekcijskoTijeloId;
        kontrola.ProizvodId = model.ProizvodId;
        kontrola.RezultatiKontrole = model.RezultatiKontrole.Trim();
        kontrola.ProizvodSiguran = model.ProizvodSiguran;

        await _context.SaveChangesAsync();

        return InspekcijskaKontrolaRezultat<InspekcijskaKontrola>.Uspjesan(kontrola);
    }

    public async Task<InspekcijskaKontrolaRezultat<InspekcijskaKontrola>> ObrisiAsync(int id)
    {
        var kontrola = await _context.InspekcijskeKontrole.FindAsync(id);
        if (kontrola is null)
        {
            return InspekcijskaKontrolaRezultat<InspekcijskaKontrola>.Neuspjesan(
                "Inspekcijska kontrola nije pronađena.");
        }

        _context.InspekcijskeKontrole.Remove(kontrola);
        await _context.SaveChangesAsync();

        return InspekcijskaKontrolaRezultat<InspekcijskaKontrola>.Uspjesan(kontrola);
    }

    private InspekcijskaKontrolaRezultat<InspekcijskaKontrola>? ProvjeriDatum(DateTime? datum)
    {
        if (datum is null)
        {
            return InspekcijskaKontrolaRezultat<InspekcijskaKontrola>.Neuspjesan(
                "Datum i vrijeme kontrole su obavezni.",
                "DatumKontrole");
        }

        var trenutnoSarajevo = TimeZoneInfo.ConvertTime(
            _timeProvider.GetUtcNow(),
            SarajevoVremenskaZona);
        var sadaBezOffseta = new DateTime(
            trenutnoSarajevo.Year,
            trenutnoSarajevo.Month,
            trenutnoSarajevo.Day,
            trenutnoSarajevo.Hour,
            trenutnoSarajevo.Minute,
            trenutnoSarajevo.Second,
            DateTimeKind.Unspecified);

        if (datum.Value > sadaBezOffseta)
        {
            return InspekcijskaKontrolaRezultat<InspekcijskaKontrola>.Neuspjesan(
                "Datum kontrole ne može biti u budućnosti.",
                "DatumKontrole");
        }

        return null;
    }

    private async Task<InspekcijskaKontrolaRezultat<InspekcijskaKontrola>?> ProvjeriTijeloIProizvodAsync(
        InspekcijskaKontrolaFormModel model)
    {
        if (!await _context.InspekcijskaTijela.AnyAsync(x => x.Id == model.InspekcijskoTijeloId))
        {
            return InspekcijskaKontrolaRezultat<InspekcijskaKontrola>.Neuspjesan(
                "Izabrano inspekcijsko tijelo ne postoji.",
                nameof(model.InspekcijskoTijeloId));
        }

        if (!await _context.Proizvodi.AnyAsync(x => x.Id == model.ProizvodId))
        {
            return InspekcijskaKontrolaRezultat<InspekcijskaKontrola>.Neuspjesan(
                "Izabrani proizvod ne postoji.",
                nameof(model.ProizvodId));
        }

        return null;
    }
}
