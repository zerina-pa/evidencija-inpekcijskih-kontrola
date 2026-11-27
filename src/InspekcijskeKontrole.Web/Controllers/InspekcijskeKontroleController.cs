using InspekcijskeKontrole.Web.Data;
using InspekcijskeKontrole.Web.Services;
using InspekcijskeKontrole.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InspekcijskeKontrole.Web.Controllers;

public class InspekcijskeKontroleController : Controller
{
    private static readonly TimeZoneInfo SarajevoVremenskaZona =
        TimeZoneInfo.FindSystemTimeZoneById("Europe/Sarajevo");

    private readonly IInspekcijskaKontrolaService _kontrolaService;
    private readonly ApplicationDbContext _context;
    private readonly TimeProvider _timeProvider;

    public InspekcijskeKontroleController(
        IInspekcijskaKontrolaService kontrolaService,
        ApplicationDbContext context,
        TimeProvider timeProvider)
    {
        _kontrolaService = kontrolaService;
        _context = context;
        _timeProvider = timeProvider;
    }

    public async Task<IActionResult> Index()
    {
        var kontrole = await _kontrolaService.DohvatiListuAsync();
        return View(kontrole);
    }

    public async Task<IActionResult> Details(int id)
    {
        var kontrola = await _kontrolaService.DohvatiPoIdAsync(id);
        return kontrola is null ? NotFound() : View(kontrola);
    }

    public async Task<IActionResult> Create()
    {
        await PopuniListeAsync();
        var sada = TrenutnoSarajevo();
        var model = new InspekcijskaKontrolaFormModel
        {
            DatumKontrole = sada
        };
        ViewBag.MaxDatum = sada.ToString("yyyy-MM-ddTHH:mm");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(InspekcijskaKontrolaFormModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopuniListeAsync(model.InspekcijskoTijeloId, model.ProizvodId);
            ViewBag.MaxDatum = TrenutnoSarajevo().ToString("yyyy-MM-ddTHH:mm");
            return View(model);
        }

        var rezultat = await _kontrolaService.KreirajAsync(model);
        if (!rezultat.Uspjeh)
        {
            DodajGrešku(rezultat.Poruka!, rezultat.Polje);
            await PopuniListeAsync(model.InspekcijskoTijeloId, model.ProizvodId);
            ViewBag.MaxDatum = TrenutnoSarajevo().ToString("yyyy-MM-ddTHH:mm");
            return View(model);
        }

        TempData["Poruka"] = "Inspekcijska kontrola je uspješno kreirana.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var kontrola = await _kontrolaService.DohvatiPoIdAsync(id);
        if (kontrola is null)
        {
            return NotFound();
        }

        var model = new InspekcijskaKontrolaFormModel
        {
            Id = kontrola.Id,
            DatumKontrole = kontrola.DatumKontrole,
            InspekcijskoTijeloId = kontrola.InspekcijskoTijeloId,
            ProizvodId = kontrola.ProizvodId,
            RezultatiKontrole = kontrola.RezultatiKontrole,
            ProizvodSiguran = kontrola.ProizvodSiguran
        };

        await PopuniListeAsync(model.InspekcijskoTijeloId, model.ProizvodId);
        ViewBag.MaxDatum = TrenutnoSarajevo().ToString("yyyy-MM-ddTHH:mm");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, InspekcijskaKontrolaFormModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await PopuniListeAsync(model.InspekcijskoTijeloId, model.ProizvodId);
            ViewBag.MaxDatum = TrenutnoSarajevo().ToString("yyyy-MM-ddTHH:mm");
            return View(model);
        }

        var rezultat = await _kontrolaService.IzmijeniAsync(id, model);
        if (!rezultat.Uspjeh)
        {
            DodajGrešku(rezultat.Poruka!, rezultat.Polje);
            await PopuniListeAsync(model.InspekcijskoTijeloId, model.ProizvodId);
            ViewBag.MaxDatum = TrenutnoSarajevo().ToString("yyyy-MM-ddTHH:mm");
            return View(model);
        }

        TempData["Poruka"] = "Inspekcijska kontrola je uspješno izmijenjena.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var kontrola = await _kontrolaService.DohvatiPoIdAsync(id);
        return kontrola is null ? NotFound() : View(kontrola);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var rezultat = await _kontrolaService.ObrisiAsync(id);
        if (!rezultat.Uspjeh)
        {
            var kontrola = await _kontrolaService.DohvatiPoIdAsync(id);
            if (kontrola is null)
            {
                return NotFound();
            }

            ModelState.AddModelError(string.Empty, rezultat.Poruka!);
            return View("Delete", kontrola);
        }

        TempData["Poruka"] = "Inspekcijska kontrola je uspješno obrisana.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopuniListeAsync(
        int odabranoTijelo = 0,
        int odabraniProizvod = 0)
    {
        var tijela = await _context.InspekcijskaTijela
            .OrderBy(x => x.Naziv)
            .ToListAsync();
        var proizvodi = await _context.Proizvodi
            .OrderBy(x => x.Naziv)
            .ToListAsync();

        ViewBag.InspekcijskaTijela = new SelectList(
            tijela,
            "Id",
            "Naziv",
            odabranoTijelo);
        ViewBag.Proizvodi = new SelectList(
            proizvodi,
            "Id",
            "Naziv",
            odabraniProizvod);
    }

    private DateTime TrenutnoSarajevo()
    {
        var trenutno = TimeZoneInfo.ConvertTime(
            _timeProvider.GetUtcNow(),
            SarajevoVremenskaZona);

        return new DateTime(
            trenutno.Year,
            trenutno.Month,
            trenutno.Day,
            trenutno.Hour,
            trenutno.Minute,
            0,
            DateTimeKind.Unspecified);
    }

    private void DodajGrešku(string poruka, string? polje)
    {
        ModelState.AddModelError(polje ?? string.Empty, poruka);
    }
}
