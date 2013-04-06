using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication15
{
    public class Kisi
    {
        public string Ad, Soyad;
        public int TC;
        
        public Kisi(Kisi kisi)
        {
            Ad = kisi.Ad;
            Soyad = kisi.Soyad;
            TC = kisi.TC;
        }

        public Kisi() { 
        
        }

        public string KendiniTanit()
        {
            return this.Ad + " " + Soyad + " adlı ve " + TC + "  TC'li kişi ";
        }
    }
}
