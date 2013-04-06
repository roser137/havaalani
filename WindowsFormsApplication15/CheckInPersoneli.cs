using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication15
{
    public class CheckinPersoneli : Personel
    {
        public CheckinPersoneli(Kisi kisi) : base(kisi as Personel)
        {
        }
    }
}
