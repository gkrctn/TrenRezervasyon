using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace TrenRez_1.Models
{
    public class RezervasyonSonuc
    {
        public bool RezervasyonYapilabilir { get; set; }
        public YerlesimAyrinti[] YerlesimAyrinti { get; set; }
    }
}
