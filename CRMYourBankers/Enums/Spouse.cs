using System.ComponentModel;

namespace CRMYourBankers.Enums
{
    public enum Spouse
    {
        [Description("Małżeństwo Wspólność")]
        MaritalCommonality,
        [Description("Małżeństwo Rozdzielność")]
        MaritalSeparation,
        [Description("Kawaler / Panna")]
        Single,
        [Description("Rozwód")]
        Divorced,
        [Description("Wdowa / Wdowiec")]
        Widower
    }
}
