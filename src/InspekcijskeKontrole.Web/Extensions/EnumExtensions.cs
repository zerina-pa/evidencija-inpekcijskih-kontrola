using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace InspekcijskeKontrole.Web.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum vrijednost)
    {
        var clan = vrijednost.GetType().GetMember(vrijednost.ToString()).FirstOrDefault();
        return clan?.GetCustomAttribute<DisplayAttribute>()?.Name
            ?? vrijednost.ToString();
    }
}
