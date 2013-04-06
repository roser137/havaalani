using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication15
{
    public class KulePersoneli : Personel
    {
        Havaalani havaalani;
        private Kisi kisi;

        public KulePersoneli(Kisi kisi) : base (kisi as Personel)
        {
        }

        public void PilotlaKonus(Pilot pilot, string mesaj) { 
        
        }

        public Hangar  inis_izni_ver(Pilot pilot) {
            var bos_Hangar = (from Hangar h in havaalani.Hangarlar
                              where h.Ucak == null
                              select h).ToList<Hangar>();
            return bos_Hangar.FirstOrDefault<Hangar>();
        }

        public bool KalkisIzniVer() {
            return false;
        }
    }
}
