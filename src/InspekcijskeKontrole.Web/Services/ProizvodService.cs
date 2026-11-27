using InspekcijskeKontrole.Web.Data;
using InspekcijskeKontrole.Web.Models;
using InspekcijskeKontrole.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InspekcijskeKontrole.Web.Services;

public class ProizvodService : IProizvodService
{
    private readonly ApplicationDbContext _context;

    public ProizvodService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Proizvod>> DohvatiListuAsync(string? pretraga)
    {
        var upit = _context.Proizvodi.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pretraga))
        {
            var tekst = pretraga.Trim();
            upit = upit.Where(x => x.Naziv.Contains(tekst));
        }

        return await upit
            .OrderBy(x => x.Naziv)
            .ToListAsync();
    }

    public Task<Proizvod?> DohvatiPoIdAsync(int id) =>
        _context.Proizvodi.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<ProizvodRezultat<Proizvod>> KreirajAsync(ProizvodFormModel model)
    {
        var serijskiBroj = NormalizujSerijskiBroj(model.SerijskiBroj);

        if (serijskiBroj is not null &&
            await _context.Proizvodi.AnyAsync(x => x.SerijskiBroj == serijskiBroj))
        {
            return ProizvodRezultat<Proizvod>.Neuspjesan(
                "Serijski broj mora biti jedinstven.",
                nameof(model.SerijskiBroj));
        }

        var proizvod = new Proizvod
        {
            Naziv = model.Naziv.Trim(),
            Proizvodjac = model.Proizvodjac.Trim(),
            SerijskiBroj = serijskiBroj,
            ZemljaPorijekla = model.ZemljaPorijekla.Trim(),
            Opis = string.IsNullOrWhiteSpace(model.Opis) ? null : model.Opis.Trim()
        };

        _context.Proizvodi.Add(proizvod);
        await _context.SaveChangesAsync();

        return ProizvodRezultat<Proizvod>.Uspjesan(proizvod);
    }

    public async Task<ProizvodRezultat<Proizvod>> IzmijeniAsync(int id, ProizvodFormModel model)
    {
        var proizvod = await _context.Proizvodi.FindAsync(id);
        if (proizvod is null)
        {
            return ProizvodRezultat<Proizvod>.Neuspjesan("Proizvod nije pronađen.");
        }

        var serijskiBroj = NormalizujSerijskiBroj(model.SerijskiBroj);

        if (serijskiBroj is not null &&
            await _context.Proizvodi.AnyAsync(x =>
                x.Id != id && x.SerijskiBroj == serijskiBroj))
        {
            return ProizvodRezultat<Proizvod>.Neuspjesan(
                "Serijski broj mora biti jedinstven.",
                nameof(model.SerijskiBroj));
        }

        proizvod.Naziv = model.Naziv.Trim();
        proizvod.Proizvodjac = model.Proizvodjac.Trim();
        proizvod.SerijskiBroj = serijskiBroj;
        proizvod.ZemljaPorijekla = model.ZemljaPorijekla.Trim();
        proizvod.Opis = string.IsNullOrWhiteSpace(model.Opis) ? null : model.Opis.Trim();

        await _context.SaveChangesAsync();

        return ProizvodRezultat<Proizvod>.Uspjesan(proizvod);
    }

    public async Task<ProizvodRezultat<Proizvod>> ObrisiAsync(int id)
    {
        var proizvod = await _context.Proizvodi
            .Include(x => x.Kontrole)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (proizvod is null)
        {
            return ProizvodRezultat<Proizvod>.Neuspjesan("Proizvod nije pronađen.");
        }

        if (proizvod.Kontrole.Count > 0)
        {
            return ProizvodRezultat<Proizvod>.Neuspjesan(
                "Proizvod nije moguće obrisati jer ima evidentirane inspekcijske kontrole.");
        }

        _context.Proizvodi.Remove(proizvod);
        await _context.SaveChangesAsync();

        return ProizvodRezultat<Proizvod>.Uspjesan(proizvod);
    }

    private static string? NormalizujSerijskiBroj(string? vrijednost)
    {
        var tekst = vrijednost?.Trim();
        return string.IsNullOrWhiteSpace(tekst) ? null : tekst;
    }
}
