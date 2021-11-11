using System.ComponentModel;

namespace CRMYourBankers.Enums
{
    public enum ClientStatus
    {
        [Description("Wstępnie Zainteresowany")]
        InitiallyInterested,
        [Description("Aktywny")]
        Active,
        [Description("Nieaktywny")]
        Inactive,
    }
}
