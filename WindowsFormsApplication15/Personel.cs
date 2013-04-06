using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication15
{
    public class Personel : Kisi
    {
        public int PersonelNo;

        public Personel(Personel p)
        {
            Ad = p.Ad;
            Soyad = p.Soyad;
            TC = p.TC;
            PersonelNo = p.PersonelNo;
            
        }

        public Personel() { 
        
        }

        
    }
}
