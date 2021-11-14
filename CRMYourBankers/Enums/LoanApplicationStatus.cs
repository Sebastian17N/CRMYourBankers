using System.ComponentModel;

namespace CRMYourBankers.Enums
{
    public enum LoanApplicationStatus
    {
        [Description("Złożony")]
        Submited,
        [Description("Wstępna oferta")]
        Offer,
        [Description("Decyzja kredytowa")]
        CreditDecision,
        [Description("Kredyt Uruchomiony")]
        Launched,
        [Description("Negat")]
        Negatives,
        [Description("Klient Zrezygnował")]
        ClientsResignation,
    }
}
