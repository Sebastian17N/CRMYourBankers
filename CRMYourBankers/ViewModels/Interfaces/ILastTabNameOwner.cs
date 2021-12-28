using CRMYourBankers.Enums;
using CRMYourBankers.Models.Interfaces;

namespace CRMYourBankers.ViewModels.Interfaces
{
    public interface ILastTabNameOwner
    {
        public TabName LastTabName { get; set; }
        public IEditable LastTabObject { get; set; }
    }
}
