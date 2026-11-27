using System.ComponentModel.DataAnnotations;

namespace InspekcijskeKontrole.Web.ViewModels;

public class InspekcijskaKontrolaFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Datum i vrijeme kontrole su obavezni.")]
    [Display(Name = "Datum i vrijeme kontrole")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? DatumKontrole { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Izaberite inspekcijsko tijelo.")]
    [Display(Name = "Inspekcijsko tijelo")]
    public int InspekcijskoTijeloId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Izaberite proizvod.")]
    [Display(Name = "Kontrolisani proizvod")]
    public int ProizvodId { get; set; }

    [Required(ErrorMessage = "Rezultati kontrole su obavezni.")]
    [StringLength(2000, ErrorMessage = "Rezultati kontrole mogu imati najviše 2000 znakova.")]
    [Display(Name = "Rezultati kontrole")]
    public string RezultatiKontrole { get; set; } = string.Empty;

    [Display(Name = "Proizvod siguran")]
    public bool ProizvodSiguran { get; set; }
}
