using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication15
{
    public class Ucus
    {
        public List<Yolcu> Yolcular;
        public DateTime KalkisZamani, VarisZamani;
        public string KalkisYeri, VarisYeri, SeferNo;
        public UcusDurumu Durum;
        public UcusTipi ucusTipi;
        public Ucak ucak;

        public Ucus() {
            Yolcular = new List<Yolcu>();
        }

        public override string ToString()
        {
            return SeferNo + " " + KalkisYeri + " " + VarisYeri + " " + '\n'.ToString() +
                KalkisZamani.ToShortDateString() + " " + KalkisZamani.ToShortTimeString() + " / " +
                VarisZamani.ToShortTimeString();
        }
    }
}
