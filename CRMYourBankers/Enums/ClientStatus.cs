using System.ComponentModel;

namespace CRMYourBankers.Enums
{
    public enum ClientStatus
    {
        [Description("Wstępnie Zainteresowany")]
        InitiallyInterested,
        [Description("Pilny Temat")]
        Urgent,
        [Description("Aktywny")]
        Active,
        [Description("Nieaktywny")]
        Inactive,
    }
}
