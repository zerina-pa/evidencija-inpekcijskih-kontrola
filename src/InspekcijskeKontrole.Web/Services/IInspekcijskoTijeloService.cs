using InspekcijskeKontrole.Web.Models;
using InspekcijskeKontrole.Web.ViewModels;

namespace InspekcijskeKontrole.Web.Services;

public interface IInspekcijskoTijeloService
{
    Task<IReadOnlyList<InspekcijskoTijelo>> DohvatiListuAsync();

    Task<InspekcijskoTijelo?> DohvatiPoIdAsync(int id);

    Task<InspekcijskoTijeloRezultat<InspekcijskoTijelo>> KreirajAsync(InspekcijskoTijeloFormModel model);

    Task<InspekcijskoTijeloRezultat<InspekcijskoTijelo>> IzmijeniAsync(
        int id,
        InspekcijskoTijeloFormModel model);

    Task<InspekcijskoTijeloRezultat<InspekcijskoTijelo>> ObrisiAsync(int id);
}
