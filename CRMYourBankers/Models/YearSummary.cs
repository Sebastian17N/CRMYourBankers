using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMYourBankers.Models
{
    public class YearSummary
    {
        public int Id { get; set; }
        public DateTime Year { get; set; }
        public string YearDisplay
            => Year.ToString("yyyy");
    }
}
