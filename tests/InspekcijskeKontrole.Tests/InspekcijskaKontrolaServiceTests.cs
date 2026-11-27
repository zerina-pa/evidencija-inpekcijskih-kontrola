using InspekcijskeKontrole.Web.Data;
using InspekcijskeKontrole.Web.Models;
using InspekcijskeKontrole.Web.Services;
using InspekcijskeKontrole.Web.ViewModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace InspekcijskeKontrole.Tests;

public class InspekcijskaKontrolaServiceTests
{
    private static readonly DateTimeOffset SadaUtc =
        new(2026, 11, 26, 9, 0, 0, TimeSpan.Zero);

    [Theory]
    [InlineData(2026, 11, 27, 9, 0, false)]
    [InlineData(2026, 11, 26, 10, 30, false)]
    [InlineData(2026, 11, 26, 10, 0, true)]
    [InlineData(2026, 11, 25, 14, 0, true)]
    public async Task KreirajAsync_PP01_PregledBuducegaIVazanogDatuma(
        int godina,
        int mjesec,
        int dan,
        int sat,
        int minuta,
        bool ocekivano)
    {
        await using var test = await KontrolaTestBaza.KreirajAsync(SadaUtc);

        var model = test.ValidanModel(
            new DateTime(godina, mjesec, dan, sat, minuta, 0, DateTimeKind.Unspecified));

        var rezultat = await test.Service.KreirajAsync(model);

        Assert.Equal(ocekivano, rezultat.Uspjeh);
        if (!ocekivano)
        {
            Assert.Equal("Datum kontrole ne može biti u budućnosti.", rezultat.Poruka);
            Assert.Equal(nameof(InspekcijskaKontrolaFormModel.DatumKontrole), rezultat.Polje);
        }
    }

    [Fact]
    public async Task IzmijeniAsync_DatumUBuducnosti_VracaGrešku()
    {
        await using var test = await KontrolaTestBaza.KreirajAsync(SadaUtc);

        var kreiranje = await test.Service.KreirajAsync(
            test.ValidanModel(new DateTime(2026, 11, 25, 14, 0, 0)));
        Assert.True(kreiranje.Uspjeh);

        var izmjena = test.ValidanModel(
            new DateTime(2026, 11, 27, 9, 0, 0, DateTimeKind.Unspecified));
        var rezultat = await test.Service.IzmijeniAsync(kreiranje.Podatak!.Id, izmjena);

        Assert.False(rezultat.Uspjeh);
        Assert.Equal("Datum kontrole ne može biti u budućnosti.", rezultat.Poruka);
    }

    [Fact]
    public async Task KreirajAsync_NepostojeciTijelo_VracaGrešku()
    {
        await using var test = await KontrolaTestBaza.KreirajAsync(SadaUtc);
        var model = test.ValidanModel(new DateTime(2026, 11, 25, 14, 0, 0));
        model.InspekcijskoTijeloId = 999;

        var rezultat = await test.Service.KreirajAsync(model);

        Assert.False(rezultat.Uspjeh);
        Assert.Equal(nameof(InspekcijskaKontrolaFormModel.InspekcijskoTijeloId), rezultat.Polje);
    }

    [Fact]
    public async Task KreirajAsync_NepostojeciProizvod_VracaGrešku()
    {
        await using var test = await KontrolaTestBaza.KreirajAsync(SadaUtc);
        var model = test.ValidanModel(new DateTime(2026, 11, 25, 14, 0, 0));
        model.ProizvodId = 999;

        var rezultat = await test.Service.KreirajAsync(model);

        Assert.False(rezultat.Uspjeh);
        Assert.Equal(nameof(InspekcijskaKontrolaFormModel.ProizvodId), rezultat.Polje);
    }

    [Fact]
    public async Task DohvatiListuAsync_SortiraOpadajucePoDatumu()
    {
        await using var test = await KontrolaTestBaza.KreirajAsync(SadaUtc);

        await test.Service.KreirajAsync(
            test.ValidanModel(new DateTime(2026, 11, 25, 14, 0, 0)));
        await test.Service.KreirajAsync(
            test.ValidanModel(new DateTime(2026, 11, 26, 9, 0, 0)));

        var lista = await test.Service.DohvatiListuAsync();

        Assert.Equal(2, lista.Count);
        Assert.True(lista[0].DatumKontrole >= lista[1].DatumKontrole);
        Assert.NotNull(lista[0].InspekcijskoTijelo);
        Assert.NotNull(lista[0].Proizvod);
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

    private sealed class KontrolaTestBaza : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;

        private KontrolaTestBaza(
            SqliteConnection connection,
            ApplicationDbContext context,
            InspekcijskaKontrolaService service,
            InspekcijskoTijelo tijelo,
            Proizvod proizvod)
        {
            _connection = connection;
            Context = context;
            Service = service;
            Tijelo = tijelo;
            Proizvod = proizvod;
        }

        public ApplicationDbContext Context { get; }

        public InspekcijskaKontrolaService Service { get; }

        public InspekcijskoTijelo Tijelo { get; }

        public Proizvod Proizvod { get; }

        public static async Task<KontrolaTestBaza> KreirajAsync(DateTimeOffset utcVrijeme)
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            var tijelo = new InspekcijskoTijelo
            {
                Naziv = "Federalna inspekcija",
                Inspektorat = Inspektorat.FBiH,
                Nadleznost = Nadleznost.TrzisnaInspekcija,
                KontaktOsoba = "Kontakt"
            };
            var proizvod = new Proizvod
            {
                Naziv = "Test proizvod",
                Proizvodjac = "Test proizvođač",
                ZemljaPorijekla = "Bosna i Hercegovina"
            };

            context.InspekcijskaTijela.Add(tijelo);
            context.Proizvodi.Add(proizvod);
            await context.SaveChangesAsync();

            var service = new InspekcijskaKontrolaService(
                context,
                new StalniTimeProvider(utcVrijeme));

            return new KontrolaTestBaza(connection, context, service, tijelo, proizvod);
        }

        public InspekcijskaKontrolaFormModel ValidanModel(DateTime datum)
        {
            return new InspekcijskaKontrolaFormModel
            {
                DatumKontrole = datum,
                InspekcijskoTijeloId = Tijelo.Id,
                ProizvodId = Proizvod.Id,
                RezultatiKontrole = "Rezultati testne kontrole",
                ProizvodSiguran = true
            };
        }

        public async ValueTask DisposeAsync()
        {
            await Context.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}
