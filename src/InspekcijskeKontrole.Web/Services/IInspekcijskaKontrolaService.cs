using InspekcijskeKontrole.Web.Models;
using InspekcijskeKontrole.Web.ViewModels;

namespace InspekcijskeKontrole.Web.Services;

public interface IInspekcijskaKontrolaService
{
    Task<IReadOnlyList<InspekcijskaKontrola>> DohvatiListuAsync();

    Task<InspekcijskaKontrola?> DohvatiPoIdAsync(int id);

    Task<InspekcijskaKontrolaRezultat<InspekcijskaKontrola>> KreirajAsync(
        InspekcijskaKontrolaFormModel model);

    Task<InspekcijskaKontrolaRezultat<InspekcijskaKontrola>> IzmijeniAsync(
        int id,
        InspekcijskaKontrolaFormModel model);

    Task<InspekcijskaKontrolaRezultat<InspekcijskaKontrola>> ObrisiAsync(int id);
}
