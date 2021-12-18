using System.ComponentModel;

namespace CRMYourBankers.Enums
{
    public enum LoanApplicationStatus
    {
        [Description("Złożony")]
        Submited,
        [Description("Wstępna Oferta")]
        Offer,
        [Description("Decyzja Kredytowa")]
        CreditDecision,
        [Description("Kredyt Uruchomiony")]
        Launched,
        [Description("Negat")]
        Negatives,
        [Description("Klient Zrezygnował")]
        ClientsResignation,
    }
}
