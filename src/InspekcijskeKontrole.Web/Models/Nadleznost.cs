using System.ComponentModel.DataAnnotations;

namespace InspekcijskeKontrole.Web.Models;

public enum Nadleznost
{
    [Display(Name = "Tržišna inspekcija")]
    TrzisnaInspekcija,

    [Display(Name = "Zdravstveno-sanitarna inspekcija")]
    ZdravstvenoSanitarnaInspekcija
}
