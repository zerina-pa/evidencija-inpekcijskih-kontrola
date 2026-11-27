using InspekcijskeKontrole.Web.Data;
using InspekcijskeKontrole.Web.Models;
using InspekcijskeKontrole.Web.Services;
using InspekcijskeKontrole.Web.ViewModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace InspekcijskeKontrole.Tests;

public class ProizvodServiceTests
{
    [Fact]
    public async Task KreirajAsync_DupliraniSerijskiBroj_VracaGrešku()
    {
        await using var test = await ProizvodTestBaza.KreirajAsync();

        await test.Service.KreirajAsync(NoviModel("PRVI-001"));
        var rezultat = await test.Service.KreirajAsync(NoviModel("  PRVI-001  "));

        Assert.False(rezultat.Uspjeh);
        Assert.Equal(nameof(ProizvodFormModel.SerijskiBroj), rezultat.Polje);
        Assert.Equal(1, await test.Context.Proizvodi.CountAsync());
    }

    [Fact]
    public async Task IzmijeniAsync_DupliraniSerijskiBroj_VracaGrešku()
    {
        await using var test = await ProizvodTestBaza.KreirajAsync();

        var prvi = await test.Service.KreirajAsync(NoviModel("PRVI-001"));
        var drugi = await test.Service.KreirajAsync(NoviModel("DRUGI-002"));

        var rezultat = await test.Service.IzmijeniAsync(
            drugi.Podatak!.Id,
            NoviModel("PRVI-001"));

        Assert.False(rezultat.Uspjeh);
        Assert.Equal(nameof(ProizvodFormModel.SerijskiBroj), rezultat.Polje);
    }

    [Fact]
    public async Task KreirajAsync_ViseProizvodaBezSerijskogBroja_DoVoljeno()
    {
        await using var test = await ProizvodTestBaza.KreirajAsync();

        var prvi = await test.Service.KreirajAsync(NoviModel("   "));
        var drugi = await test.Service.KreirajAsync(NoviModel(string.Empty));

        Assert.True(prvi.Uspjeh);
        Assert.True(drugi.Uspjeh);
        Assert.Null(prvi.Podatak!.SerijskiBroj);
        Assert.Null(drugi.Podatak!.SerijskiBroj);
        Assert.Equal(2, await test.Context.Proizvodi.CountAsync());
    }

    [Fact]
    public async Task ObrisiAsync_ProizvodSaKontrolom_VracaGrešku()
    {
        await using var test = await ProizvodTestBaza.KreirajAsync();

        var proizvodRezultat = await test.Service.KreirajAsync(NoviModel("OBRISI-001"));

        var tijelo = new InspekcijskoTijelo
        {
            Naziv = "Federalna inspekcija",
            Inspektorat = Inspektorat.FBiH,
            Nadleznost = Nadleznost.TrzisnaInspekcija,
            KontaktOsoba = "Kontakt"
        };

        test.Context.InspekcijskaTijela.Add(tijelo);
        test.Context.InspekcijskeKontrole.Add(new InspekcijskaKontrola
        {
            DatumKontrole = DateTime.UtcNow,
            InspekcijskoTijelo = tijelo,
            Proizvod = proizvodRezultat.Podatak!,
            RezultatiKontrole = "Kontrola je evidentirana.",
            ProizvodSiguran = true
        });
        await test.Context.SaveChangesAsync();

        var rezultat = await test.Service.ObrisiAsync(proizvodRezultat.Podatak!.Id);

        Assert.False(rezultat.Uspjeh);
        Assert.Contains("evidentirane inspekcijske kontrole", rezultat.Poruka);
        Assert.True(await test.Context.Proizvodi.AnyAsync(x => x.Id == proizvodRezultat.Podatak.Id));
    }

    [Fact]
    public async Task ObrisiAsync_ProizvodBezKontrole_Brise()
    {
        await using var test = await ProizvodTestBaza.KreirajAsync();

        var proizvodRezultat = await test.Service.KreirajAsync(NoviModel("OBRISI-002"));
        var rezultat = await test.Service.ObrisiAsync(proizvodRezultat.Podatak!.Id);

        Assert.True(rezultat.Uspjeh);
        Assert.False(await test.Context.Proizvodi.AnyAsync(x => x.Id == proizvodRezultat.Podatak.Id));
    }

    private static ProizvodFormModel NoviModel(string? serijskiBroj)
    {
        return new ProizvodFormModel
        {
            Naziv = "Test proizvod",
            Proizvodjac = "Test proizvođač",
            SerijskiBroj = serijskiBroj,
            ZemljaPorijekla = "Bosna i Hercegovina",
            Opis = "Opis proizvoda"
        };
    }

    private sealed class ProizvodTestBaza : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;

        private ProizvodTestBaza(SqliteConnection connection, ApplicationDbContext context)
        {
            _connection = connection;
            Context = context;
            Service = new ProizvodService(context);
        }

        public ApplicationDbContext Context { get; }

        public ProizvodService Service { get; }

        public static async Task<ProizvodTestBaza> KreirajAsync()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            return new ProizvodTestBaza(connection, context);
        }

        public async ValueTask DisposeAsync()
        {
            await Context.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}
