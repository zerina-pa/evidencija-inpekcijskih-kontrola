using InspekcijskeKontrole.Web.Data;
using InspekcijskeKontrole.Web.Services;
using InspekcijskeKontrole.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InspekcijskeKontrole.Web.Controllers;

public class IzvjestajiController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IIzvjestajService _izvjestajService;

    public IzvjestajiController(
        ApplicationDbContext context,
        IIzvjestajService izvjestajService)
    {
        _context = context;
        _izvjestajService = izvjestajService;
    }

    [HttpGet]
    public async Task<IActionResult> KontrolePoTijelu(
        IzvjestajKontrolaParametriViewModel parametri)
    {
        await PopuniTijelaAsync(parametri.InspekcijskoTijeloId);

        if (parametri.InspekcijskoTijeloId == 0 &&
            parametri.DatumOd is null &&
            parametri.DatumDo is null)
        {
            return View("KontrolePoTijeluParametri", parametri);
        }

        if (!TryValidateModel(parametri))
        {
            return View("KontrolePoTijeluParametri", parametri);
        }

        if (parametri.DatumOd > parametri.DatumDo)
        {
            ModelState.AddModelError(
                nameof(parametri.DatumOd),
                "Datum od ne može biti poslije datuma do.");
            return View("KontrolePoTijeluParametri", parametri);
        }

        var rezultat = await _izvjestajService
            .DohvatiKontrolePoTijeluZaPeriodAsync(
                parametri.InspekcijskoTijeloId,
                parametri.DatumOd!.Value,
                parametri.DatumDo!.Value);

        if (!rezultat.Uspjeh)
        {
            if (rezultat.Polje == nameof(parametri.DatumOd))
            {
                ModelState.AddModelError(nameof(parametri.DatumOd), rezultat.Poruka!);
            }
            else
            {
                ModelState.AddModelError(
                    rezultat.Polje ?? string.Empty,
                    rezultat.Poruka!);
            }

            return View("KontrolePoTijeluParametri", parametri);
        }

        return View("KontrolePoTijelu", rezultat.Podatak);
    }

    [HttpGet]
    public async Task<IActionResult> Zapisnik(int id)
    {
        var zapisnik = await _izvjestajService.DohvatiZapisnikPoIdAsync(id);
        if (zapisnik is null)
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
            return View("NijePronadjeno");
        }

        return View(zapisnik);
    }

    private async Task PopuniTijelaAsync(int odabranoTijelo)
    {
        var tijela = await _context.InspekcijskaTijela
            .OrderBy(x => x.Naziv)
            .ToListAsync();

        ViewBag.InspekcijskaTijela = new SelectList(
            tijela,
            "Id",
            "Naziv",
            odabranoTijelo);
    }
}
