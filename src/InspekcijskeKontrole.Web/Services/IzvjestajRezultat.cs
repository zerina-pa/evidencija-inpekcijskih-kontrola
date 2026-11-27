namespace InspekcijskeKontrole.Web.Services;

public class IzvjestajRezultat<T>
{
    private IzvjestajRezultat(bool uspjeh, T? podatak, string? poruka, string? polje)
    {
        Uspjeh = uspjeh;
        Podatak = podatak;
        Poruka = poruka;
        Polje = polje;
    }

    public bool Uspjeh { get; }

    public T? Podatak { get; }

    public string? Poruka { get; }

    public string? Polje { get; }

    public static IzvjestajRezultat<T> Uspjesan(T podatak) =>
        new(true, podatak, null, null);

    public static IzvjestajRezultat<T> Neuspjesan(string poruka, string? polje = null) =>
        new(false, default, poruka, polje);
}
