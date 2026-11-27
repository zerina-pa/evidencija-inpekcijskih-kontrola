using System.ComponentModel.DataAnnotations;

namespace InspekcijskeKontrole.Web.ViewModels;

public class ProizvodFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Naziv proizvoda je obavezan.")]
    [StringLength(200, ErrorMessage = "Naziv proizvoda može imati najviše 200 znakova.")]
    [Display(Name = "Naziv proizvoda")]
    public string Naziv { get; set; } = string.Empty;

    [Required(ErrorMessage = "Proizvođač je obavezan.")]
    [StringLength(200, ErrorMessage = "Proizvođač može imati najviše 200 znakova.")]
    [Display(Name = "Proizvođač")]
    public string Proizvodjac { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Serijski broj može imati najviše 100 znakova.")]
    [Display(Name = "Serijski broj")]
    public string? SerijskiBroj { get; set; }

    [Required(ErrorMessage = "Zemlja porijekla je obavezna.")]
    [StringLength(100, ErrorMessage = "Zemlja porijekla može imati najviše 100 znakova.")]
    [Display(Name = "Zemlja porijekla")]
    public string ZemljaPorijekla { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Opis može imati najviše 2000 znakova.")]
    [Display(Name = "Opis")]
    public string? Opis { get; set; }
}
