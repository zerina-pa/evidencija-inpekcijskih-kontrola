using InspekcijskeKontrole.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace InspekcijskeKontrole.Web.Data;

public static class SeedPodataka
{
    private static readonly TimeZoneInfo SarajevoVremenskaZona =
        TimeZoneInfo.FindSystemTimeZoneById("Europe/Sarajevo");

    public static async Task NapuniAkoJePraznoAsync(
        ApplicationDbContext context,
        TimeProvider timeProvider)
    {
        if (await context.InspekcijskaTijela.AnyAsync() ||
            await context.Proizvodi.AnyAsync() ||
            await context.InspekcijskeKontrole.AnyAsync())
        {
            return;
        }

        var tijela = new[]
        {
            new InspekcijskoTijelo
            {
                Naziv = "Federalna uprava za inspekcijske poslove",
                Inspektorat = Inspektorat.FBiH,
                Nadleznost = Nadleznost.TrzisnaInspekcija,
                KontaktOsoba = "Amra Hodžić"
            },
            new InspekcijskoTijelo
            {
                Naziv = "Federalna inspekcija zdravstva",
                Inspektorat = Inspektorat.FBiH,
                Nadleznost = Nadleznost.ZdravstvenoSanitarnaInspekcija,
                KontaktOsoba = "Dr. Selma Mehmedović"
            },
            new InspekcijskoTijelo
            {
                Naziv = "Republika inspekcija Republike Srpske",
                Inspektorat = Inspektorat.RS,
                Nadleznost = Nadleznost.TrzisnaInspekcija,
                KontaktOsoba = "Milan Petrović"
            },
            new InspekcijskoTijelo
            {
                Naziv = "Inspekcijski ured Distrikta Brčko",
                Inspektorat = Inspektorat.DistriktBrcko,
                Nadleznost = Nadleznost.ZdravstvenoSanitarnaInspekcija,
                KontaktOsoba = "Jasmina Hasanbegović"
            }
        };

        var proizvodi = new[]
        {
            new Proizvod
            {
                Naziv = "Mineralna voda Zlatno vrelo",
                Proizvodjac = "Zlatno vrelo d.o.o.",
                SerijskiBroj = "ZV-2026-001",
                ZemljaPorijekla = "Bosna i Hercegovina",
                Opis = "Negazirana prirodna mineralna voda, pakovanje 1,5 l."
            },
            new Proizvod
            {
                Naziv = "Dječija sočiva s vitaminima",
                Proizvodjac = "Jadran Nutritiv",
                SerijskiBroj = "JN-CH-2405",
                ZemljaPorijekla = "Hrvatska",
                Opis = "Dodatak prehrani u prahu."
            },
            new Proizvod
            {
                Naziv = "Energijska lanterna EL-300",
                Proizvodjac = "Elektronik Nova",
                SerijskiBroj = "EL300-7781",
                ZemljaPorijekla = "Srbija",
                Opis = "Prenosiva LED lampa sa punjivom baterijom."
            },
            new Proizvod
            {
                Naziv = "Maslinovo ulje Extra",
                Proizvodjac = "Mediteran Quality",
                SerijskiBroj = "MQ-EX-553",
                ZemljaPorijekla = "Italija",
                Opis = "Maslinovo ulje ekološkog porijekla, pakovanje 750 ml."
            },
            new Proizvod
            {
                Naziv = "Pasterizirano mlijeko 2.8%",
                Proizvodjac = "Livanjska mljekara",
                SerijskiBroj = null,
                ZemljaPorijekla = "Bosna i Hercegovina",
                Opis = "Pasterizirano kravlje mlijeko u tetrapak ambalaži."
            },
            new Proizvod
            {
                Naziv = "Bezbrižni sapun",
                Proizvodjac = "Natur Kosmetika",
                SerijskiBroj = "BK-S24-188",
                ZemljaPorijekla = "Slovenija",
                Opis = "Toaletni sapun za svakodnevnu upotrebu."
            },
            new Proizvod
            {
                Naziv = "Smrznuti grašak 1 kg",
                Proizvodjac = "Green Valley",
                SerijskiBroj = "GV-GR-120",
                ZemljaPorijekla = "Crna Gora",
                Opis = "Brzo smrznut zeleni grašak."
            },
            new Proizvod
            {
                Naziv = "Bluetooth slušalice SoundLite",
                Proizvodjac = "Digital Products",
                SerijskiBroj = "SL-BT-9007",
                ZemljaPorijekla = "Turska",
                Opis = "Bežične slušalice sa aktivnim smanjenjem buke."
            }
        };

        await context.InspekcijskaTijela.AddRangeAsync(tijela);
        await context.Proizvodi.AddRangeAsync(proizvodi);
        await context.SaveChangesAsync();

        var sadaUtc = timeProvider.GetUtcNow();
        var trenutnoSarajevo = TimeZoneInfo.ConvertTime(sadaUtc, SarajevoVremenskaZona);
        var danas = new DateTime(
            trenutnoSarajevo.Year,
            trenutnoSarajevo.Month,
            trenutnoSarajevo.Day,
            10,
            0,
            0,
            DateTimeKind.Unspecified);

        var kontrolniDatumi = new[]
        {
            danas.AddDays(-8).AddHours(2),
            danas.AddDays(-15).AddHours(5),
            danas.AddDays(-23).AddHours(3),
            danas.AddDays(-31).AddHours(7),
            danas.AddDays(-45).AddHours(2),
            danas.AddDays(-57).AddHours(4),
            danas.AddDays(-69).AddHours(1),
            danas.AddDays(-82).AddHours(6),
            danas.AddDays(-96).AddHours(3),
            danas.AddDays(-108).AddHours(5),
            danas.AddDays(-121).AddHours(2),
            danas.AddDays(-137).AddHours(7),
            danas.AddDays(-149).AddHours(1),
            danas.AddDays(-163).AddHours(4),
            danas.AddDays(-174).AddHours(6),
            danas.AddDays(-184).AddHours(2),
            danas.AddDays(-196).AddHours(5),
            danas.AddDays(-208).AddHours(3),
            danas.AddDays(-222).AddHours(7),
            danas.AddDays(-238).AddHours(1),
            danas.AddDays(-251).AddHours(4),
            danas.AddDays(-269).AddHours(6),
            danas.AddDays(-282).AddHours(2)
        };

        var rezultati = new[]
        {
            "Ambalaža i deklaracija su u skladu. Serijski broj je evidentiran, a rok upotrebe je važeći.",
            "Uzeti uzorci odgovaraju propisima. Nije uočena nepravilnost u skladišnoj dokumentaciji.",
            "Na ambalaži nedostaje potpuna informacija o proizvođaču. Rok važenja je produžen za dopunu dokumentacije.",
            "Kontrolom je utvrđeno da proizvod neispravno radi pri visokoj temperaturi; trgovac je obaviješten.",
            "Etiketa sadrži sve obavezne podatke na bosanskom jeziku. Proizvod je bez vidljivih oštećenja.",
            "Uzorak je odstupio u periodu održivosti. Preporučeno povlačenje serije iz prodaje.",
            "Naloženo je privremeno povlačenje proizvoda zbog oštećene ambalaže i nedostajućeg serijskog broja.",
            "Dokumentacija lanca opskrbe je potpuna. Na mjestu kontrole nije pronađena nepravilnost.",
            "Uočeno je neusklađeno datumiranje na više komada. Proizvođač je obaviješten o korektivnoj radnji.",
            "Laboratorijska analiza nije ukazala na povećanu prisutnost štetnih supstanci.",
            "Kontrola skladišnih uslova je zadovoljavajuća. Temperatura i vlažnost su u propisanom rasponu.",
            "Ambalaža je oštećena na manjim dijelovima; proizvod je izdvojen iz ponude.",
            "Korisničke upute su potpune i razumljive. Nema razloga za zabranu puštanja u promet.",
            "Testiran je random uzorak od 10 komada; svi su prošli vizuelnu i funkcionalnu provjeru.",
            "Utvrđena je nesaglasnost u deklaraciji zemalja porijekla. Izdana je preporuka za ispravku.",
            "Proizvod je usklađen sa važećim zahtjevima i može ostati u prodaji.",
            "Kontrola bezvizualnog sloja ambalaže je prošla, ali je zabilježen nedostatan broj deklaracija.",
            "Mjerenja kvaliteta odgovaraju deklarisanim vrijednostima.",
            "Uočena nepravilnost je povučena nakon korektivne radnje izvođača.",
            "Zaključano je da nema sigurnosnog rizika za krajnje korisnike.",
            "Serija je označena jasno i omogućava praćenje porekla.",
            "Površina ambalaže i zatvarač nisu bili oštećeni.",
            "Kontrolni uzorak je preuzet za laboratorijsku provjeru.",
            "Kontrola je završena bez nepravilnosti i bez potrebe za daljnjim mjerama."
        };

        var kontrole = new List<InspekcijskaKontrola>();
        for (var i = 0; i < kontrolniDatumi.Length; i++)
        {
            var indeksTijela = i switch
            {
                >= 0 and <= 7 => 0,
                >= 8 and <= 10 => 1,
                >= 11 and <= 12 => 2,
                _ => 3
            };

            kontrole.Add(new InspekcijskaKontrola
            {
                DatumKontrole = kontrolniDatumi[i],
                InspekcijskoTijelo = tijela[indeksTijela],
                Proizvod = proizvodi[i % proizvodi.Length],
                RezultatiKontrole = rezultati[i % rezultati.Length],
                ProizvodSiguran = i % 4 != 2 && i % 7 != 5
            });
        }

        await context.InspekcijskeKontrole.AddRangeAsync(kontrole);
        await context.SaveChangesAsync();
    }
}
