using CRMYourBankers.Enums;
using CRMYourBankers.Models.Interfaces;

namespace CRMYourBankers.Messages
{
    public class TabChangeMessage
    {
        public TabName TabName { get; set; }
        public object SelectedObject { get; set; }
        public bool GoFurther { get; set; } = true;
    }
}
