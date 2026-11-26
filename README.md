# Evidencija inspekcijskih kontrola proizvoda

Web aplikacija za evidenciju i pregled inspekcijskih kontrola proizvoda na tržištu BiH.
Omogućava vođenje evidencije proizvoda, inspekcijskih tijela i izvršenih kontrola, te
generisanje izvještaja o kontrolama po inspekcijskom tijelu i zapisnika pojedinačne kontrole.

## Funkcionalnosti
- Evidencija proizvoda (unos, izmjena, brisanje)
- Evidencija inspekcijskih tijela (unos, izmjena, brisanje)
- Evidencija inspekcijskih kontrola, uz zabranu unosa datuma u budućnosti
- Izvještaj: kontrole izabranog inspekcijskog tijela za izabrani period (s parametarskom formom)
- Izvještaj: detalji izabrane inspekcijske kontrole

## Tehnologije
- ASP.NET Core MVC (.NET 8)
- Entity Framework Core + SQLite
- Bootstrap 5
- xUnit (testovi)

## Pokretanje
> TODO: dopuniti kada aplikacija bude spremna (preduslovi, komande, demo podaci).

## Dokumentacija
- [Funkcionalni zahtjevi](docs/funkcionalni_zahtjevi.md)
- [Nefunkcionalni zahtjevi](docs/nefunkcionalni_zahtjevi.md)
- [User stories](docs/user_stories.md)
- [Model podataka (ERD)](docs/erd.md)

## Struktura repozitorija
| Folder | Sadržaj |
|---|---|
| `docs/` | Zahtjevi i dijagrami |
| `src/` | Izvorni kod aplikacije |
| `tests/` | Automatski testovi |
