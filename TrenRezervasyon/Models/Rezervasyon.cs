using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrenRez_1.Models
{
    public class Rezervasyon
    {
        public Tren Tren { get; set; }
        public int RezervasyonYapilacakKisiSayisi { get; set; }
        public bool KisilerFarkliVagonlaraYerlestirilebilir { get; set; }

    }
}
