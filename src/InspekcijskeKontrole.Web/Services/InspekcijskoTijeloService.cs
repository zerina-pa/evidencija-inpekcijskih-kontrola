using InspekcijskeKontrole.Web.Data;
using InspekcijskeKontrole.Web.Models;
using InspekcijskeKontrole.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InspekcijskeKontrole.Web.Services;

public class InspekcijskoTijeloService : IInspekcijskoTijeloService
{
    private readonly ApplicationDbContext _context;

    public InspekcijskoTijeloService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<InspekcijskoTijelo>> DohvatiListuAsync()
    {
        return await _context.InspekcijskaTijela
            .OrderBy(x => x.Naziv)
            .ToListAsync();
    }

    public Task<InspekcijskoTijelo?> DohvatiPoIdAsync(int id) =>
        _context.InspekcijskaTijela.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<InspekcijskoTijeloRezultat<InspekcijskoTijelo>> KreirajAsync(
        InspekcijskoTijeloFormModel model)
    {
        var enumRezultat = ProvjeriEnumVrijednosti(model);
        if (enumRezultat is not null)
        {
            return enumRezultat;
        }

        var tijelo = new InspekcijskoTijelo
        {
            Naziv = model.Naziv.Trim(),
            Inspektorat = model.Inspektorat,
            Nadleznost = model.Nadleznost,
            KontaktOsoba = model.KontaktOsoba.Trim()
        };

        _context.InspekcijskaTijela.Add(tijelo);
        await _context.SaveChangesAsync();

        return InspekcijskoTijeloRezultat<InspekcijskoTijelo>.Uspjesan(tijelo);
    }

    public async Task<InspekcijskoTijeloRezultat<InspekcijskoTijelo>> IzmijeniAsync(
        int id,
        InspekcijskoTijeloFormModel model)
    {
        var enumRezultat = ProvjeriEnumVrijednosti(model);
        if (enumRezultat is not null)
        {
            return enumRezultat;
        }

        var tijelo = await _context.InspekcijskaTijela.FindAsync(id);
        if (tijelo is null)
        {
            return InspekcijskoTijeloRezultat<InspekcijskoTijelo>.Neuspjesan(
                "Inspekcijsko tijelo nije pronađeno.");
        }

        tijelo.Naziv = model.Naziv.Trim();
        tijelo.Inspektorat = model.Inspektorat;
        tijelo.Nadleznost = model.Nadleznost;
        tijelo.KontaktOsoba = model.KontaktOsoba.Trim();

        await _context.SaveChangesAsync();

        return InspekcijskoTijeloRezultat<InspekcijskoTijelo>.Uspjesan(tijelo);
    }

    public async Task<InspekcijskoTijeloRezultat<InspekcijskoTijelo>> ObrisiAsync(int id)
    {
        var tijelo = await _context.InspekcijskaTijela
            .Include(x => x.Kontrole)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (tijelo is null)
        {
            return InspekcijskoTijeloRezultat<InspekcijskoTijelo>.Neuspjesan(
                "Inspekcijsko tijelo nije pronađeno.");
        }

        if (tijelo.Kontrole.Count > 0)
        {
            return InspekcijskoTijeloRezultat<InspekcijskoTijelo>.Neuspjesan(
                "Inspekcijsko tijelo nije moguće obrisati jer ima evidentirane inspekcijske kontrole.");
        }

        _context.InspekcijskaTijela.Remove(tijelo);
        await _context.SaveChangesAsync();

        return InspekcijskoTijeloRezultat<InspekcijskoTijelo>.Uspjesan(tijelo);
    }

    private static InspekcijskoTijeloRezultat<InspekcijskoTijelo>? ProvjeriEnumVrijednosti(
        InspekcijskoTijeloFormModel model)
    {
        if (!Enum.IsDefined(model.Inspektorat))
        {
            return InspekcijskoTijeloRezultat<InspekcijskoTijelo>.Neuspjesan(
                "Izaberite važeći inspektorat.",
                nameof(model.Inspektorat));
        }

        if (!Enum.IsDefined(model.Nadleznost))
        {
            return InspekcijskoTijeloRezultat<InspekcijskoTijelo>.Neuspjesan(
                "Izaberite važeću nadležnost.",
                nameof(model.Nadleznost));
        }

        return null;
    }
}
