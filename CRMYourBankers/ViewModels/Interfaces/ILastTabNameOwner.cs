using CRMYourBankers.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMYourBankers.ViewModels.Interfaces
{
    public interface ILastTabNameOwner
    {
        public TabName LastTabName { get; set; }
    }
}
