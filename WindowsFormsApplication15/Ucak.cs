using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication15
{
    public class Ucak
    {
        public Pilot Pilot;
        public void inis_yap(KulePersoneli personel) {
            if (personel.inis_izni_ver(Pilot)!=null) {
                personel.inis_izni_ver(Pilot).ucak_kabul_et(this);
            } 
        }
    }
}
