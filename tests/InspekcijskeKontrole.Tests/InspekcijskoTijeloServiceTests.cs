using InspekcijskeKontrole.Web.Data;
using InspekcijskeKontrole.Web.Models;
using InspekcijskeKontrole.Web.Services;
using InspekcijskeKontrole.Web.ViewModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace InspekcijskeKontrole.Tests;

public class InspekcijskoTijeloServiceTests
{
    [Fact]
    public async Task KreirajAsync_NevalidanInspektorat_VracaGrešku()
    {
        await using var test = await TijeloTestBaza.KreirajAsync();
        var model = NoviModel();
        model.Inspektorat = (Inspektorat)99;

        var rezultat = await test.Service.KreirajAsync(model);

        Assert.False(rezultat.Uspjeh);
        Assert.Equal(nameof(InspekcijskoTijeloFormModel.Inspektorat), rezultat.Polje);
        Assert.False(await test.Context.InspekcijskaTijela.AnyAsync());
    }

    [Fact]
    public async Task KreirajAsync_NevalidnaNadleznost_VracaGrešku()
    {
        await using var test = await TijeloTestBaza.KreirajAsync();
        var model = NoviModel();
        model.Nadleznost = (Nadleznost)99;

        var rezultat = await test.Service.KreirajAsync(model);

        Assert.False(rezultat.Uspjeh);
        Assert.Equal(nameof(InspekcijskoTijeloFormModel.Nadleznost), rezultat.Polje);
        Assert.False(await test.Context.InspekcijskaTijela.AnyAsync());
    }

    [Fact]
    public async Task ObrisiAsync_TijeloSaKontrolom_VracaGrešku()
    {
        await using var test = await TijeloTestBaza.KreirajAsync();
        var rezultat = await test.Service.KreirajAsync(NoviModel());

        var proizvod = new Proizvod
        {
            Naziv = "Test proizvod",
            Proizvodjac = "Test proizvođač",
            ZemljaPorijekla = "Bosna i Hercegovina"
        };

        test.Context.Proizvodi.Add(proizvod);
        test.Context.InspekcijskeKontrole.Add(new InspekcijskaKontrola
        {
            DatumKontrole = DateTime.UtcNow,
            InspekcijskoTijelo = rezultat.Podatak!,
            Proizvod = proizvod,
            RezultatiKontrole = "Kontrola je evidentirana.",
            ProizvodSiguran = true
        });
        await test.Context.SaveChangesAsync();

        var brisanje = await test.Service.ObrisiAsync(rezultat.Podatak!.Id);

        Assert.False(brisanje.Uspjeh);
        Assert.Contains("evidentirane inspekcijske kontrole", brisanje.Poruka);
        Assert.True(await test.Context.InspekcijskaTijela
            .AnyAsync(x => x.Id == rezultat.Podatak.Id));
    }

    [Fact]
    public async Task ObrisiAsync_TijeloBezKontrole_Brise()
    {
        await using var test = await TijeloTestBaza.KreirajAsync();
        var kreiranje = await test.Service.KreirajAsync(NoviModel());

        var rezultat = await test.Service.ObrisiAsync(kreiranje.Podatak!.Id);

        Assert.True(rezultat.Uspjeh);
        Assert.False(await test.Context.InspekcijskaTijela
            .AnyAsync(x => x.Id == kreiranje.Podatak.Id));
    }

    private static InspekcijskoTijeloFormModel NoviModel()
    {
        return new InspekcijskoTijeloFormModel
        {
            Naziv = "Federalna inspekcija",
            Inspektorat = Inspektorat.FBiH,
            Nadleznost = Nadleznost.TrzisnaInspekcija,
            KontaktOsoba = "Test kontakt"
        };
    }

    private sealed class TijeloTestBaza : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;

        private TijeloTestBaza(SqliteConnection connection, ApplicationDbContext context)
        {
            _connection = connection;
            Context = context;
            Service = new InspekcijskoTijeloService(context);
        }

        public ApplicationDbContext Context { get; }

        public InspekcijskoTijeloService Service { get; }

        public static async Task<TijeloTestBaza> KreirajAsync()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            return new TijeloTestBaza(connection, context);
        }

        public async ValueTask DisposeAsync()
        {
            await Context.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}
