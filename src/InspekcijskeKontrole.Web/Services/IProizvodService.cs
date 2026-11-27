using InspekcijskeKontrole.Web.Models;
using InspekcijskeKontrole.Web.ViewModels;

namespace InspekcijskeKontrole.Web.Services;

public interface IProizvodService
{
    Task<IReadOnlyList<Proizvod>> DohvatiListuAsync(string? pretraga);

    Task<Proizvod?> DohvatiPoIdAsync(int id);

    Task<ProizvodRezultat<Proizvod>> KreirajAsync(ProizvodFormModel model);

    Task<ProizvodRezultat<Proizvod>> IzmijeniAsync(int id, ProizvodFormModel model);

    Task<ProizvodRezultat<Proizvod>> ObrisiAsync(int id);
}
