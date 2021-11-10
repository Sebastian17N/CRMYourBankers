using System.ComponentModel;

namespace CRMYourBankers.Enums
{
    public enum ZusUs
    {
        [Description("Nie zalega")]
        DontArrear,
        [Description("Zalega")]
        Arrear,
        [Description("Układ ratalny")]
        InstallmentPlan
    }
}
