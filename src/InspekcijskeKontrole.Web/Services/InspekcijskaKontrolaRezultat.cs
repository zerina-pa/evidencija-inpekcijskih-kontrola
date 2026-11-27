namespace InspekcijskeKontrole.Web.Services;

public class InspekcijskaKontrolaRezultat<T>
{
    private InspekcijskaKontrolaRezultat(bool uspjeh, T? podatak, string? poruka, string? polje)
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

    public static InspekcijskaKontrolaRezultat<T> Uspjesan(T podatak) =>
        new(true, podatak, null, null);

    public static InspekcijskaKontrolaRezultat<T> Neuspjesan(string poruka, string? polje = null) =>
        new(false, default, poruka, polje);
}
