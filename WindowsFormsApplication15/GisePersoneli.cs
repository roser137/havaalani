using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication15
{
    public class GisePersoneli : Personel
    {
      
        public void BiletVer() { }

        public GisePersoneli() { }

        public GisePersoneli(Personel p) : base(p) {
        
        }
        
        public GisePersoneli(Kisi kisi)
        {
            this.Ad = kisi.Ad;
            this.Soyad = kisi.Soyad;
            this.TC = kisi.TC;
            
        }

        public Bilet BiletVer(Yolcu y, Ucus u) {
            return new Bilet(y, u);
        }
    }
}
