using System.ComponentModel;

namespace CRMYourBankers.Enums
{
    public enum SourceOfIncome
    {
        [Description("Działaność KPIR")]
        KPIR,
        [Description("Działaność Ryczałt")]
        Ryczałt,
        [Description("Działaność Ryczałt (do zmiany)")]
        RyczałtChange,
        [Description("Działaność Karta podatkowa")]
        TaxCard,
        [Description("Osoba zatrudniona")]
        EmployedPerson,
        [Description("Emerytura / renta")]
        Retiring,
        [Description("Spółka z o.o.")]
        LTD,
    }
}
