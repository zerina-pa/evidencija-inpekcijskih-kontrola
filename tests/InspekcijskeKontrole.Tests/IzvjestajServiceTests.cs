using InspekcijskeKontrole.Web.Data;
using InspekcijskeKontrole.Web.Models;
using InspekcijskeKontrole.Web.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace InspekcijskeKontrole.Tests;

public class IzvjestajServiceTests
{
    [Fact]
    public async Task KontrolaNaDanDo18UlaziUPeriod()
    {
        await using var test = await IzvjestajTestBaza.KreirajAsync();
        await test.DodajKontroluAsync(test.PrvoTijelo, test.Proizvod,
            new DateTime(2026, 11, 20, 18, 0, 0));

        var rezultat = await test.Service.DohvatiKontrolePoTijeluZaPeriodAsync(
            test.PrvoTijelo.Id,
            new DateOnly(2026, 11, 20),
            new DateOnly(2026, 11, 20));

        Assert.True(rezultat.Uspjeh);
        Assert.Single(rezultat.Podatak!.Kontrole);
    }

    [Fact]
    public async Task KontrolaDanPoslijeDo00NeUlaziUPeriod()
    {
        await using var test = await IzvjestajTestBaza.KreirajAsync();
        await test.DodajKontroluAsync(test.PrvoTijelo, test.Proizvod,
            new DateTime(2026, 11, 21, 0, 0, 0));

        var rezultat = await test.Service.DohvatiKontrolePoTijeluZaPeriodAsync(
            test.PrvoTijelo.Id,
            new DateOnly(2026, 11, 20),
            new DateOnly(2026, 11, 20));

        Assert.True(rezultat.Uspjeh);
        Assert.Empty(rezultat.Podatak!.Kontrole);
    }

    [Fact]
    public async Task KontroleDrugogTijelaNeUlaze()
    {
        await using var test = await IzvjestajTestBaza.KreirajAsync();
        await test.DodajKontroluAsync(test.PrvoTijelo, test.Proizvod,
            new DateTime(2026, 11, 20, 10, 0, 0));
        await test.DodajKontroluAsync(test.DrugoTijelo, test.Proizvod,
            new DateTime(2026, 11, 20, 12, 0, 0));

        var rezultat = await test.Service.DohvatiKontrolePoTijeluZaPeriodAsync(
            test.PrvoTijelo.Id,
            new DateOnly(2026, 11, 20),
            new DateOnly(2026, 11, 20));

        Assert.Single(rezultat.Podatak!.Kontrole);
        Assert.All(rezultat.Podatak!.Kontrole,
            x => Assert.Equal(test.PrvoTijelo.Id, x.InspekcijskoTijeloId));
    }

    [Fact]
    public async Task RezultatJeSortiranRastucePoDatumu()
    {
        await using var test = await IzvjestajTestBaza.KreirajAsync();
        await test.DodajKontroluAsync(test.PrvoTijelo, test.Proizvod,
            new DateTime(2026, 11, 20, 16, 0, 0));
        await test.DodajKontroluAsync(test.PrvoTijelo, test.Proizvod,
            new DateTime(2026, 11, 20, 8, 0, 0));

        var rezultat = await test.Service.DohvatiKontrolePoTijeluZaPeriodAsync(
            test.PrvoTijelo.Id,
            new DateOnly(2026, 11, 20),
            new DateOnly(2026, 11, 20));

        Assert.Equal(2, rezultat.Podatak!.Kontrole.Count);
        Assert.True(rezultat.Podatak!.Kontrole[0].DatumKontrole <
                    rezultat.Podatak!.Kontrole[1].DatumKontrole);
    }

    [Fact]
    public async Task DatumOdPoslijeDatumDo_VracaPP04Grešku()
    {
        await using var test = await IzvjestajTestBaza.KreirajAsync();

        var rezultat = await test.Service.DohvatiKontrolePoTijeluZaPeriodAsync(
            test.PrvoTijelo.Id,
            new DateOnly(2026, 11, 21),
            new DateOnly(2026, 11, 20));

        Assert.False(rezultat.Uspjeh);
        Assert.Equal("Datum od ne može biti poslije datuma do.", rezultat.Poruka);
        Assert.Equal("DatumOd", rezultat.Polje);
    }

    [Fact]
    public async Task PrazanPeriod_VracaPrazanRezultat()
    {
        await using var test = await IzvjestajTestBaza.KreirajAsync();

        var rezultat = await test.Service.DohvatiKontrolePoTijeluZaPeriodAsync(
            test.PrvoTijelo.Id,
            new DateOnly(2026, 11, 20),
            new DateOnly(2026, 11, 20));

        Assert.True(rezultat.Uspjeh);
        Assert.Empty(rezultat.Podatak!.Kontrole);
        Assert.Equal(0, rezultat.Podatak!.UkupanBrojKontrola);
    }

    [Fact]
    public async Task Zapisnik_SadrziTraženaPolja()
    {
        await using var test = await IzvjestajTestBaza.KreirajAsync();
        test.Proizvod.SerijskiBroj = "SBR-123";
        await test.Context.SaveChangesAsync();

        var kontrolaId = await test.DodajKontroluAsyncVratiId(
            test.PrvoTijelo,
            test.Proizvod,
            new DateTime(2026, 11, 20, 18, 0, 0));

        var zapisnik = await test.Service.DohvatiZapisnikPoIdAsync(kontrolaId);

        Assert.NotNull(zapisnik);
        Assert.Equal("SBR-123", zapisnik!.SerijskiBroj);
        Assert.Equal("Test proizvod", zapisnik.NazivProizvoda);
        Assert.Equal("Bosna i Hercegovina", zapisnik.ZemljaPorijekla);
        Assert.Equal(new DateTime(2026, 11, 20, 18, 0, 0), zapisnik.DatumKontrole);
        Assert.Equal("Rezultat", zapisnik.RezultatiKontrole);
        Assert.Equal("Federalna inspekcija", zapisnik.NazivInspekcijskogTijela);
        Assert.True(zapisnik.ProizvodSiguran);
    }

    [Fact]
    public async Task Zapisnik_BezSerijskogBroja_KoristiCrtic()
    {
        await using var test = await IzvjestajTestBaza.KreirajAsync();
        test.Proizvod.SerijskiBroj = null;
        await test.Context.SaveChangesAsync();

        var kontrolaId = await test.DodajKontroluAsyncVratiId(
            test.PrvoTijelo,
            test.Proizvod,
            new DateTime(2026, 11, 20, 18, 0, 0));

        var zapisnik = await test.Service.DohvatiZapisnikPoIdAsync(kontrolaId);

        Assert.NotNull(zapisnik);
        Assert.Equal("–", zapisnik!.SerijskiBroj);
    }

    [Fact]
    public async Task Zapisnik_NepostojećiId_VracaNull()
    {
        await using var test = await IzvjestajTestBaza.KreirajAsync();

        var zapisnik = await test.Service.DohvatiZapisnikPoIdAsync(999);

        Assert.Null(zapisnik);
    }

    private sealed class StalniTimeProvider : TimeProvider
    {
        private readonly DateTimeOffset _utcVrijeme;

        public StalniTimeProvider(DateTimeOffset utcVrijeme)
        {
            _utcVrijeme = utcVrijeme;
        }

        public override DateTimeOffset GetUtcNow() => _utcVrijeme;
    }

    private sealed class IzvjestajTestBaza : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;

        private IzvjestajTestBaza(
            SqliteConnection connection,
            ApplicationDbContext context,
            IzvjestajService service,
            InspekcijskoTijelo prvoTijelo,
            InspekcijskoTijelo drugoTijelo,
            Proizvod proizvod)
        {
            _connection = connection;
            Context = context;
            Service = service;
            PrvoTijelo = prvoTijelo;
            DrugoTijelo = drugoTijelo;
            Proizvod = proizvod;
        }

        public ApplicationDbContext Context { get; }

        public IzvjestajService Service { get; }

        public InspekcijskoTijelo PrvoTijelo { get; }

        public InspekcijskoTijelo DrugoTijelo { get; }

        public Proizvod Proizvod { get; }

        public static async Task<IzvjestajTestBaza> KreirajAsync()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            var prvoTijelo = new InspekcijskoTijelo
            {
                Naziv = "Federalna inspekcija",
                Inspektorat = Inspektorat.FBiH,
                Nadleznost = Nadleznost.TrzisnaInspekcija,
                KontaktOsoba = "Kontakt 1"
            };
            var drugoTijelo = new InspekcijskoTijelo
            {
                Naziv = "Republika inspekcija",
                Inspektorat = Inspektorat.RS,
                Nadleznost = Nadleznost.ZdravstvenoSanitarnaInspekcija,
                KontaktOsoba = "Kontakt 2"
            };
            var proizvod = new Proizvod
            {
                Naziv = "Test proizvod",
                Proizvodjac = "Test proizvođač",
                SerijskiBroj = "IZV-001",
                ZemljaPorijekla = "Bosna i Hercegovina"
            };

            context.InspekcijskaTijela.AddRange(prvoTijelo, drugoTijelo);
            context.Proizvodi.Add(proizvod);
            await context.SaveChangesAsync();

            var service = new IzvjestajService(
                context,
                new StalniTimeProvider(
                    new DateTimeOffset(2026, 11, 26, 9, 0, 0, TimeSpan.Zero)));

            return new IzvjestajTestBaza(
                connection,
                context,
                service,
                prvoTijelo,
                drugoTijelo,
                proizvod);
        }

        public async Task DodajKontroluAsync(
            InspekcijskoTijelo tijelo,
            Proizvod proizvod,
            DateTime datum)
        {
            Context.InspekcijskeKontrole.Add(new InspekcijskaKontrola
            {
                DatumKontrole = datum,
                InspekcijskoTijelo = tijelo,
                Proizvod = proizvod,
                RezultatiKontrole = "Rezultat",
                ProizvodSiguran = true
            });
            await Context.SaveChangesAsync();
        }

        public async Task<int> DodajKontroluAsyncVratiId(
            InspekcijskoTijelo tijelo,
            Proizvod proizvod,
            DateTime datum)
        {
            var kontrola = new InspekcijskaKontrola
            {
                DatumKontrole = datum,
                InspekcijskoTijelo = tijelo,
                Proizvod = proizvod,
                RezultatiKontrole = "Rezultat",
                ProizvodSiguran = true
            };

            Context.InspekcijskeKontrole.Add(kontrola);
            await Context.SaveChangesAsync();
            return kontrola.Id;
        }

        public async ValueTask DisposeAsync()
        {
            await Context.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}
