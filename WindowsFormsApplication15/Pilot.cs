using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication15
{
    public class Pilot : Personel
    {
        public Pilot(Kisi kisi) : base(kisi as Personel)
        {
        }
        public void InisIzniIste() { }
    }
}
