using System.Globalization;
using InspekcijskeKontrole.Web.Data;
using InspekcijskeKontrole.Web.Services;
using Microsoft.EntityFrameworkCore;

var kultura = CultureInfo.GetCultureInfo("bs-Latn-BA");
CultureInfo.DefaultThreadCurrentCulture = kultura;
CultureInfo.DefaultThreadCurrentUICulture = kultura;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' nije konfiguriran.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IProizvodService, ProizvodService>();
builder.Services.AddScoped<IInspekcijskoTijeloService, InspekcijskoTijeloService>();
builder.Services.AddScoped<IInspekcijskaKontrolaService, InspekcijskaKontrolaService>();
builder.Services.AddScoped<IIzvjestajService, IzvjestajService>();
builder.Services.AddSingleton(TimeProvider.System);

var app = builder.Build();

app.UseRequestLocalization(kultura.Name);

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    await SeedPodataka.NapuniAkoJePraznoAsync(
        dbContext,
        scope.ServiceProvider.GetRequiredService<TimeProvider>());
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
