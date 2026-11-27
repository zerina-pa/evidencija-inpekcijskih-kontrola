using InspekcijskeKontrole.Web.Services;
using InspekcijskeKontrole.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InspekcijskeKontrole.Web.Controllers;

public class InspekcijskaTijelaController : Controller
{
    private readonly IInspekcijskoTijeloService _tijeloService;

    public InspekcijskaTijelaController(IInspekcijskoTijeloService tijeloService)
    {
        _tijeloService = tijeloService;
    }

    public async Task<IActionResult> Index()
    {
        var tijela = await _tijeloService.DohvatiListuAsync();
        return View(tijela);
    }

    public async Task<IActionResult> Details(int id)
    {
        var tijelo = await _tijeloService.DohvatiPoIdAsync(id);
        return tijelo is null ? NotFound() : View(tijelo);
    }

    public IActionResult Create()
    {
        return View(new InspekcijskoTijeloFormModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(InspekcijskoTijeloFormModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var rezultat = await _tijeloService.KreirajAsync(model);
        if (!rezultat.Uspjeh)
        {
            DodajGrešku(rezultat.Poruka!, rezultat.Polje);
            return View(model);
        }

        TempData["Poruka"] = "Inspekcijsko tijelo je uspješno kreirano.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var tijelo = await _tijeloService.DohvatiPoIdAsync(id);
        if (tijelo is null)
        {
            return NotFound();
        }

        var model = new InspekcijskoTijeloFormModel
        {
            Id = tijelo.Id,
            Naziv = tijelo.Naziv,
            Inspektorat = tijelo.Inspektorat,
            Nadleznost = tijelo.Nadleznost,
            KontaktOsoba = tijelo.KontaktOsoba
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, InspekcijskoTijeloFormModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var rezultat = await _tijeloService.IzmijeniAsync(id, model);
        if (!rezultat.Uspjeh)
        {
            DodajGrešku(rezultat.Poruka!, rezultat.Polje);
            return View(model);
        }

        TempData["Poruka"] = "Inspekcijsko tijelo je uspješno izmijenjeno.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var tijelo = await _tijeloService.DohvatiPoIdAsync(id);
        return tijelo is null ? NotFound() : View(tijelo);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var rezultat = await _tijeloService.ObrisiAsync(id);
        if (!rezultat.Uspjeh)
        {
            var tijelo = await _tijeloService.DohvatiPoIdAsync(id);
            if (tijelo is null)
            {
                return NotFound();
            }

            ModelState.AddModelError(string.Empty, rezultat.Poruka!);
            return View("Delete", tijelo);
        }

        TempData["Poruka"] = "Inspekcijsko tijelo je uspješno obrisano.";
        return RedirectToAction(nameof(Index));
    }

    private void DodajGrešku(string poruka, string? polje)
    {
        ModelState.AddModelError(polje ?? string.Empty, poruka);
    }
}
