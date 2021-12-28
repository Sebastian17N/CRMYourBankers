using CRMYourBankers.Enums;
using CRMYourBankers.Models.Interfaces;

namespace CRMYourBankers.Messages
{
    public class TabChangeMessage
    {
        public TabName TabName { get; set; }
        //string zmienić na TabName i wszędzie roziwązać konflikty
        public object SelectedObject { get; set; }
        public TabName LastTabName { get; set; }
        public IEditable LastTabObject { get; set; }

        public bool GoFurther { get; set; } = true;
    }
}
