using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication15
{
    public class Havaalani
    {
        public Kule Kule;
        public List<Hangar> Hangarlar;
        public DateTime Zaman;
        // public List<Ucak> Ucaklar;
        // public List<Yolcu> Yolcular; // uçuşlarda tutmak yeterli
        public List<Ucus> Ucuslar;

        public List<Pilot> Pilotlar;
        public List<GisePersoneli> GisePersonelleri;
        // public List<KulePersoneli> KulePersonelleri;
        public List<CheckinPersoneli> CheckInPersonelleri;

        public Havaalani() {
            Hangarlar = new List<Hangar>();
            Zaman = DateTime.Now;
            // Yolcular = new List<Yolcu>();
            Ucuslar = new List<Ucus>();
            Kule = new Kule();
            Pilotlar = new List<Pilot>();
            GisePersonelleri = new List<GisePersoneli>();
            CheckInPersonelleri = new List<CheckinPersoneli>();
        }


        public Ucus UcuslariKontrolEt()
        {
            foreach (Ucus u in Ucuslar) {
                if (u.ucusTipi == UcusTipi.Inis)
                {
                    if (u.VarisZamani <= Zaman) {
                        return u;
                    }
                }
                else {
                    if (u.KalkisZamani <= Zaman) {
                        return u;
                    }
                }
            }
            return null;
        }

        public void InenUcagiHangaraYerlestir(Ucak inenUcak)
        {
            foreach (Hangar h in Hangarlar) {
                if (h.Ucak == null) {
                    h.Ucak = inenUcak;
                    break;
                }
            }
        }

        public Ucak HangardanMusaitUcakVer()
        {
            foreach (Hangar h in Hangarlar) {
                if (h.Ucak != null) {
                    Ucak u = h.Ucak;
                    h.Ucak = null;
                    return u;
                }
            }
            return null;
        }
    }
}
