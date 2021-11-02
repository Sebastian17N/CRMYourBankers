using CRMYourBankers.Enum;
using CRMYourBankers.Models;

namespace CRMYourBankers.Messages
{
    public class TabChangeMessage
    {
        public string TabName { get; set; }
        public int ObjectId { get; set; }
        public TabName LastTabName { get; set; }
    }
}
