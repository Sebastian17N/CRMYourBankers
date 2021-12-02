using System.ComponentModel;

namespace CRMYourBankers.Enums
{
    public enum BIKType
    {
        [Description("Kredyty Osobiste")] PersonalLoans,
        [Description("Zapytania Osobiste")] PersonalQuestions,
        [Description("Kredyty Firmowe")] CompanyLoans,
        [Description("Zapytania Firmowe")] CompanyQuestions
    }
}
