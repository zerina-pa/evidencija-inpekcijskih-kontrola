using System.ComponentModel.DataAnnotations;
using InspekcijskeKontrole.Web.Models;

namespace InspekcijskeKontrole.Web.ViewModels;

public class InspekcijskoTijeloFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Naziv inspekcijskog tijela je obavezan.")]
    [StringLength(200, ErrorMessage = "Naziv inspekcijskog tijela može imati najviše 200 znakova.")]
    [Display(Name = "Naziv inspekcijskog tijela")]
    public string Naziv { get; set; } = string.Empty;

    [Required(ErrorMessage = "Inspektorat je obavezan.")]
    [EnumDataType(typeof(Inspektorat), ErrorMessage = "Izaberite važeći inspektorat.")]
    [Display(Name = "Inspektorat")]
    public Inspektorat Inspektorat { get; set; }

    [Required(ErrorMessage = "Nadležnost je obavezna.")]
    [EnumDataType(typeof(Nadleznost), ErrorMessage = "Izaberite važeću nadležnost.")]
    [Display(Name = "Nadležnost")]
    public Nadleznost Nadleznost { get; set; }

    [Required(ErrorMessage = "Kontakt osoba je obavezna.")]
    [StringLength(200, ErrorMessage = "Kontakt osoba može imati najviše 200 znakova.")]
    [Display(Name = "Kontakt osoba")]
    public string KontaktOsoba { get; set; } = string.Empty;
}
