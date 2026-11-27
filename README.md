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
Preduslov je instaliran .NET 10 SDK.

1. Klonirajte repozitorij i uđite u korijenski folder projekta:
   ```powershell
   git clone <URL-REPOZITORIJUMA>
   cd evidencija-inpekcijskih-kontrola
   ```
2. Pokrenite aplikaciju iz korijenskog foldera:
   ```powershell
   dotnet run --project src/InspekcijskeKontrole.Web/InspekcijskeKontrole.Web.csproj
   ```
3. Otvorite adresu prikazanu u terminalu, obično `https://localhost:<port>` ili `http://localhost:<port>`.

Pokrenite testove iz korijenskog foldera:
```powershell
dotnet test InspekcijskeKontrole.sln
```

Za resetovanje baze, zaustavite aplikaciju i obrišite fajl `src/InspekcijskeKontrole.Web/inspekcije.db` zajedno sa opcionalnim `.db-shm` i `.db-wal` fajlovima. Ponovo pokrenite aplikaciju: migracija će kreirati praznu bazu, a FZ-06 demo podaci će se automatski učitati.

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
