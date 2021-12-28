using CRMYourBankers.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMYourBankers.ViewModels.Interfaces
{
    internal interface ISelectedItemOwner <A> where A : IEditable
    {
        A SelectedItem { get; set; }


    }
}
