using CRMYourBankers.Enums;

namespace CRMYourBankers.ViewModels.Interfaces
{
    public interface ILastTabNameOwner
    {
        public TabName LastTabName { get; set; }
        public object LastTabObject { get; set; }
    }
}
