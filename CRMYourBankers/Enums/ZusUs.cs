using System.ComponentModel.DataAnnotations;

namespace CRMYourBankers.Enums
{
    public enum ZusUs
    {
        [Display(Name="Nie zalega")]
        NieZalega,
        [Display(Name = "Zalega")]
        Zalega,
        [Display(Name = "Układ ratalny")]
        UkładRatalny
    }
}
