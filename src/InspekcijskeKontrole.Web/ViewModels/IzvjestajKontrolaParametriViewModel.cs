using System.ComponentModel.DataAnnotations;

namespace InspekcijskeKontrole.Web.ViewModels;

public class IzvjestajKontrolaParametriViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Izaberite inspekcijsko tijelo.")]
    [Display(Name = "Inspekcijsko tijelo")]
    public int InspekcijskoTijeloId { get; set; }

    [Required(ErrorMessage = "Datum od je obavezan.")]
    [Display(Name = "Datum od")]
    [DataType(DataType.Date)]
    public DateOnly? DatumOd { get; set; }

    [Required(ErrorMessage = "Datum do je obavezan.")]
    [Display(Name = "Datum do")]
    [DataType(DataType.Date)]
    public DateOnly? DatumDo { get; set; }
}
