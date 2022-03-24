using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using TrenRez_1.Models;

namespace TrenRez_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RezervasyonController : ControllerBase
    {

        public Rezervasyon RezList
        {
            get
            {
                Rezervasyon rezervasyon1 = new Rezervasyon();
                rezervasyon1.Tren = new Tren
                {
                    Ad = "Baskent Ekspres",
                    Vagonlar = new Vagon[3]
                };
                rezervasyon1.Tren.Vagonlar[0] = new Vagon() {Ad = "Vagon1", Kapasite = 100, DoluKoltukAdet = 50};
                rezervasyon1.Tren.Vagonlar[1] = new Vagon() { Ad = "Vagon2", Kapasite = 90, DoluKoltukAdet = 80 };
                rezervasyon1.Tren.Vagonlar[2] = new Vagon() { Ad = "Vagon3", Kapasite = 80, DoluKoltukAdet = 80 };

                return rezervasyon1;
            }
        }

        
        [HttpGet]
        //[Route("api/[controller]/{kriter}")]
        [Route("Getir/{kriter}")]
        public Vagon Getir(string kriter)
        {

            return RezList.Tren.Vagonlar.FirstOrDefault(a=>a.Ad==kriter);
        }

        [Route("rez")]
        [HttpPost]
        public RezervasyonSonuc Post(Rezervasyon rezervasyon)
        {
            //Vagon Sayısı
            int vagonSayi = rezervasyon.Tren.Vagonlar.Length;
            int[] bosKoltukVagon = new int[vagonSayi];
            for (int i = 0; i <= vagonSayi - 1; i++)
            {
                bosKoltukVagon[i] = (int)Math.Abs(0.7 * (rezervasyon.Tren.Vagonlar[i].Kapasite - rezervasyon.Tren.Vagonlar[i].DoluKoltukAdet));
            }

            //Toplam Boş Koltuk
            int toplamBosKoltuk = bosKoltukVagon.Sum();

            RezervasyonSonuc rezervasyonSonuc = new RezervasyonSonuc();

            if (rezervasyon.RezervasyonYapilacakKisiSayisi <= toplamBosKoltuk)
            {
                rezervasyonSonuc.RezervasyonYapilabilir = true;
                if (rezervasyon.KisilerFarkliVagonlaraYerlestirilebilir==true)
                {
                    if (rezervasyon.RezervasyonYapilacakKisiSayisi <= bosKoltukVagon[0])
                    {
                        rezervasyonSonuc.YerlesimAyrinti = new YerlesimAyrinti[1];
                        rezervasyonSonuc.YerlesimAyrinti[0] = new YerlesimAyrinti
                        {
                            VagonAdi = rezervasyon.Tren.Vagonlar[0].Ad,
                            KisiSayisi = rezervasyon.RezervasyonYapilacakKisiSayisi
                        };
                    }
                    if (rezervasyon.RezervasyonYapilacakKisiSayisi > bosKoltukVagon[0] && rezervasyon.RezervasyonYapilacakKisiSayisi <= (bosKoltukVagon[0] + bosKoltukVagon[1]))
                    {
                        rezervasyonSonuc.YerlesimAyrinti = new YerlesimAyrinti[2];
                        for (int i = 0; i <=  1; i++)
                        {
                            rezervasyonSonuc.YerlesimAyrinti[i] = new YerlesimAyrinti();
                        }
                        rezervasyonSonuc.YerlesimAyrinti[0].VagonAdi = rezervasyon.Tren.Vagonlar[0].Ad;
                        rezervasyonSonuc.YerlesimAyrinti[0].KisiSayisi = bosKoltukVagon[0];
                        rezervasyonSonuc.YerlesimAyrinti[1].VagonAdi = rezervasyon.Tren.Vagonlar[1].Ad;
                        rezervasyonSonuc.YerlesimAyrinti[1].KisiSayisi = rezervasyon.RezervasyonYapilacakKisiSayisi - bosKoltukVagon[0];
                    }
                    if(rezervasyon.RezervasyonYapilacakKisiSayisi>(bosKoltukVagon[0] + bosKoltukVagon[1]) && rezervasyon.RezervasyonYapilacakKisiSayisi<=(toplamBosKoltuk))
                    {
                        rezervasyonSonuc.YerlesimAyrinti = new YerlesimAyrinti[3];
                        for (int i = 0; i <= 2; i++)
                        {
                            rezervasyonSonuc.YerlesimAyrinti[i] = new YerlesimAyrinti();
                        }
                        rezervasyonSonuc.YerlesimAyrinti[0].VagonAdi = rezervasyon.Tren.Vagonlar[0].Ad;
                        rezervasyonSonuc.YerlesimAyrinti[0].KisiSayisi = bosKoltukVagon[0];
                        rezervasyonSonuc.YerlesimAyrinti[1].VagonAdi = rezervasyon.Tren.Vagonlar[1].Ad;
                        rezervasyonSonuc.YerlesimAyrinti[1].KisiSayisi = bosKoltukVagon[1];
                        rezervasyonSonuc.YerlesimAyrinti[2].VagonAdi = rezervasyon.Tren.Vagonlar[2].Ad;
                        rezervasyonSonuc.YerlesimAyrinti[2].KisiSayisi = rezervasyon.RezervasyonYapilacakKisiSayisi - bosKoltukVagon[0] - bosKoltukVagon[1];
                    }
                }
                if (rezervasyon.KisilerFarkliVagonlaraYerlestirilebilir != true)
                {
                    if (rezervasyon.RezervasyonYapilacakKisiSayisi <= bosKoltukVagon[0])
                    {
                        rezervasyonSonuc.YerlesimAyrinti = new YerlesimAyrinti[1];
                        rezervasyonSonuc.YerlesimAyrinti[0] = new YerlesimAyrinti
                        {
                            VagonAdi = rezervasyon.Tren.Vagonlar[0].Ad,
                            KisiSayisi = rezervasyon.RezervasyonYapilacakKisiSayisi
                        };
                    }
                    else if (rezervasyon.RezervasyonYapilacakKisiSayisi <= bosKoltukVagon[1])
                    {
                        rezervasyonSonuc.YerlesimAyrinti = new YerlesimAyrinti[1];
                        rezervasyonSonuc.YerlesimAyrinti[0] = new YerlesimAyrinti
                        {
                            VagonAdi = rezervasyon.Tren.Vagonlar[1].Ad,
                            KisiSayisi = rezervasyon.RezervasyonYapilacakKisiSayisi
                        };
                    }
                    else if (rezervasyon.RezervasyonYapilacakKisiSayisi <= bosKoltukVagon[2])
                    {
                        rezervasyonSonuc.YerlesimAyrinti = new YerlesimAyrinti[1];
                        rezervasyonSonuc.YerlesimAyrinti[0] = new YerlesimAyrinti
                        {
                            VagonAdi = rezervasyon.Tren.Vagonlar[2].Ad,
                            KisiSayisi = rezervasyon.RezervasyonYapilacakKisiSayisi
                        };
                    }
                    else
                    {
                        rezervasyonSonuc.RezervasyonYapilabilir = false;
                        rezervasyonSonuc.YerlesimAyrinti = new YerlesimAyrinti[0];
                    }
                }
            }
            else
            {
                rezervasyonSonuc.RezervasyonYapilabilir = false;
                rezervasyonSonuc.YerlesimAyrinti = new YerlesimAyrinti[0];
            }
            return rezervasyonSonuc;
        }
    }
}
