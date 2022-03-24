using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrenRez_1.Models
{
    public class Tren
    {
        //public int Id { get; set; }
        public string Ad { get; set; }
        public Vagon[] Vagonlar { get; set; }
    }
}
