using CRMYourBankers.Enums;

namespace CRMYourBankers.Messages
{
    public class TabChangeMessage
    {
        public TabName TabName { get; set; }
        //string zmienić na TabName i wszędzie roziwązać konflikty
        public int ObjectId { get; set; }
        public TabName LastTabName { get; set; }
    }
}
