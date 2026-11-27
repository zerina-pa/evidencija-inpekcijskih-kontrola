using System.ComponentModel.DataAnnotations;

namespace InspekcijskeKontrole.Web.Models;

public enum Inspektorat
{
    [Display(Name = "FBiH")]
    FBiH,

    [Display(Name = "RS")]
    RS,

    [Display(Name = "Distrikt Brčko")]
    DistriktBrcko
}
