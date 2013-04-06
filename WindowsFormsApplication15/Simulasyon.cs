using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication15
{
    public class Simulasyon
    {
        Havaalani havaAlani;
        Random rastgele;

        string[] adlar =    { "Mehmet", "muhammed", "Sercan", "Ahmet", "Fatih", "Alper", "Murat"};
        string[] soyadlar = { "Yılmaz", "Pak", "Duru", "Tunç", "Can", "Kursun", "Kucuk"};
        string[] sehirler = { "Malatya", "İstanbul", "Ankara", "Adana", "Antalya", "İzmir", "Trabzon" };
        string[] sefer = { "PGS", "OGS", "MKR", "THY", "TK", "NR", "ATL" };

        public Simulasyon(Havaalani havaAlani) {
            this.havaAlani = havaAlani;
            rastgele = new Random();
        }

        public void BaslangicDurumunaGetir(){
            int i, rast;
            rast = rastgele.Next(1, 4);
            for (i = 0; i < rast; i++)
            {
                CheckinPersoneli checkinPersoneli = new CheckinPersoneli(rastgeleKisiOlustur(KisiTipi.Personel));
                havaAlani.CheckInPersonelleri.Add(checkinPersoneli);
            }
            rast = rastgele.Next(1, 4);
            for (i = 0; i < rast; i++)
            {
                // TODO: cast ederek neden olmuyor sor.
                GisePersoneli gisePersoneli = new GisePersoneli(rastgeleKisiOlustur(KisiTipi.Personel) as Personel);
                havaAlani.GisePersonelleri.Add(gisePersoneli);
            }

            for (i = 0; i < 6; i++)
            {
                havaAlani.Hangarlar.Add(new Hangar() { Ucak=null });
            }
            
            rast = rastgele.Next(1, 4);
            for (i = 0; i < rast; i++)
            {
                KulePersoneli kulePersoneli = new KulePersoneli(rastgeleKisiOlustur(KisiTipi.Personel));
                havaAlani.Kule.KulePersonelleri.Add(kulePersoneli);
            }

            rast = rastgele.Next(0, 7);
            for (i = 0; i < rast; i++)
            {
                Pilot pilot = new Pilot(rastgeleKisiOlustur(KisiTipi.Personel));
                havaAlani.Pilotlar.Add(pilot);
            }

            rast = rastgele.Next(5, 10);
            for (i = 0; i < rast; i++)
            {
                bool gidenmi;
                if (rastgele.Next(0, 2) == 1) gidenmi = true;
                else gidenmi = false;
                Ucus ucus = rastgeleUcusOlustur(gidenmi);
                havaAlani.Ucuslar.Add(ucus);
            }


            // TODO: devamm..
        }
        
        public Kisi rastgeleKisiOlustur(KisiTipi kisiTipi) {
            Kisi kisi = new Kisi();
            kisi.Ad = adlar[rastgele.Next(0, adlar.Length - 1)];
            kisi.Soyad = soyadlar[rastgele.Next(0, soyadlar.Length - 1)];
            kisi.TC = rastgele.Next(100000000, 900000000);

            if (kisiTipi == KisiTipi.Yolcu)
            {
                // TODO: neden exception fırlatıyor sor
                // Yolcu yolcu = (kisi as Yolcu);
                Yolcu yolcu = new Yolcu();
                yolcu.Ad = kisi.Ad;
                yolcu.Soyad = kisi.Soyad;
                yolcu.TC = kisi.TC;
                yolcu.Ucus = null;
                return yolcu;
            }
            else if (kisiTipi == KisiTipi.Personel)
            {
                Personel personel = new Personel();
                personel.Ad = kisi.Ad;
                personel.Soyad = kisi.Soyad;
                personel.TC = kisi.TC;
                personel.PersonelNo = rastgele.Next(10000, 90000);
                return personel;
            }


            return kisi;
        }

        private Ucus rastgeleUcusOlustur(bool GidenMi) {
            int yolcuSayisi = rastgele.Next(2, 5);
            Ucak ucak = new Ucak();
            ucak.Pilot = rastgeleKisiOlustur(KisiTipi.Personel) as Pilot;
            
            Ucus ucus = new Ucus() { 
                Durum = UcusDurumu.Beklemede,
                SeferNo = sefer[rastgele.Next(0, sefer.Length - 1)] + rastgele.Next(10000,90000),
                ucak = ucak
            };

            if (GidenMi)
            {
                ucus.KalkisYeri = "Edirne";
                ucus.VarisYeri = sehirler[rastgele.Next(0, sehirler.Length - 1)];
                ucus.ucusTipi = UcusTipi.Kalkis;
            }
            else {
                ucus.KalkisYeri = sehirler[rastgele.Next(0, sehirler.Length - 1)];
                ucus.VarisYeri = "Edirne";
                ucus.ucusTipi = UcusTipi.Inis;
            }

            for (int i = 0; i < yolcuSayisi; i++) {
                Yolcu yolcu = new Yolcu(rastgeleKisiOlustur(KisiTipi.Yolcu));
                yolcu.Ucus = ucus;
                ucus.Yolcular.Add(yolcu);
            }

            ucus.KalkisZamani = DateTime.Now.AddHours(rastgele.Next(0, 240));
            ucus.VarisZamani = ucus.KalkisZamani.AddHours(rastgele.Next(1, 3));
            

            return ucus;
        }

        


    }
}
