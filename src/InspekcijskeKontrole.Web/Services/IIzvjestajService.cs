using InspekcijskeKontrole.Web.ViewModels;

namespace InspekcijskeKontrole.Web.Services;

public interface IIzvjestajService
{
    Task<IzvjestajRezultat<IzvjestajKontrolaViewModel>> DohvatiKontrolePoTijeluZaPeriodAsync(
        int inspekcijskoTijeloId,
        DateOnly datumOd,
        DateOnly datumDo);

    Task<ZapisnikKontroleViewModel?> DohvatiZapisnikPoIdAsync(int id);
}
