using InspekcijskeKontrole.Web.Services;
using InspekcijskeKontrole.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InspekcijskeKontrole.Web.Controllers;

public class ProizvodiController : Controller
{
    private readonly IProizvodService _proizvodService;

    public ProizvodiController(IProizvodService proizvodService)
    {
        _proizvodService = proizvodService;
    }

    public async Task<IActionResult> Index(string? pretraga)
    {
        var proizvodi = await _proizvodService.DohvatiListuAsync(pretraga);
        ViewBag.Pretraga = pretraga;
        return View(proizvodi);
    }

    public async Task<IActionResult> Details(int id)
    {
        var proizvod = await _proizvodService.DohvatiPoIdAsync(id);
        return proizvod is null ? NotFound() : View(proizvod);
    }

    public IActionResult Create()
    {
        return View(new ProizvodFormModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProizvodFormModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var rezultat = await _proizvodService.KreirajAsync(model);
        if (!rezultat.Uspjeh)
        {
            if (rezultat.Polje is not null)
            {
                ModelState.AddModelError(rezultat.Polje, rezultat.Poruka!);
            }
            else
            {
                ModelState.AddModelError(string.Empty, rezultat.Poruka!);
            }

            return View(model);
        }

        TempData["Poruka"] = "Proizvod je uspješno kreiran.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var proizvod = await _proizvodService.DohvatiPoIdAsync(id);
        if (proizvod is null)
        {
            return NotFound();
        }

        var model = new ProizvodFormModel
        {
            Id = proizvod.Id,
            Naziv = proizvod.Naziv,
            Proizvodjac = proizvod.Proizvodjac,
            SerijskiBroj = proizvod.SerijskiBroj,
            ZemljaPorijekla = proizvod.ZemljaPorijekla,
            Opis = proizvod.Opis
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProizvodFormModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var rezultat = await _proizvodService.IzmijeniAsync(id, model);
        if (!rezultat.Uspjeh)
        {
            if (rezultat.Polje is not null)
            {
                ModelState.AddModelError(rezultat.Polje, rezultat.Poruka!);
            }
            else
            {
                ModelState.AddModelError(string.Empty, rezultat.Poruka!);
            }

            return View(model);
        }

        TempData["Poruka"] = "Proizvod je uspješno izmijenjen.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var proizvod = await _proizvodService.DohvatiPoIdAsync(id);
        return proizvod is null ? NotFound() : View(proizvod);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var rezultat = await _proizvodService.ObrisiAsync(id);
        if (!rezultat.Uspjeh)
        {
            var proizvod = await _proizvodService.DohvatiPoIdAsync(id);
            if (proizvod is null)
            {
                return NotFound();
            }

            ModelState.AddModelError(string.Empty, rezultat.Poruka!);
            return View("Delete", proizvod);
        }

        TempData["Poruka"] = "Proizvod je uspješno obrisan.";
        return RedirectToAction(nameof(Index));
    }
}
