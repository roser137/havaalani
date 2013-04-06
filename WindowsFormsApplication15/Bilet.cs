using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication15
{
    public class Bilet
    {
        private Random rnd;
        public int BiletNo;
        public Yolcu yolcu;
        public Ucus ucus;

        public Bilet() {
            rnd = new Random();
        }

        public Bilet(Yolcu yolcu, Ucus ucus) {
            rnd = new Random();
            this.yolcu = yolcu;
            this.ucus = ucus;
            BiletNo = rnd.Next(100000, 900000);
        }
    }
}
