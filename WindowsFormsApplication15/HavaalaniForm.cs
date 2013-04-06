using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication15.Properties;
using System.Media;

namespace WindowsFormsApplication15
{
    public partial class HavaalaniForm : Form
    {
        public Havaalani havaalani;
        public Simulasyon simulasyon;

        private bool spHandeOynatildiMi = false;
        private bool spKizanOynatildiMi = false;

        private SoundPlayer spHande, spKizan;
        private List<PictureBox> pbYolcular, pbYolcular2; // animasyon için
        private Ucus ZamaniGelenUcus; // kontrol için
        private int musteriBekle = 0; // animasyon için     
        private bool biletAlmaAnimasyonuOynatiliyor = false; // animasyon tamamlanmadan tekrar başlatılmaması için
        private bool ucakKalkisAnimasyonuOynatiliyor = false;
        private bool ucakInisAnimasyonuOynatiliyor = false;
        private Random rnd = new Random();
        private int rand;
        private int indirilecekYolcuSayisi, bindirilecekYolcuSayisi;
        private bool kirmizimi = false; // ışık animasyonları için
        private Ucak inenUcak, kalkanUcak;


        public HavaalaniForm()
        {
            InitializeComponent();
        }


        private void FormHavaAlani_Load(object sender, EventArgs e)
        {
            // Resmin flip edilmiş hali lazım. 
            pbMusteri2.Image.RotateFlip(RotateFlipType.Rotate180FlipY);
            // ve başta görünmemesi gerekiyor.
            pbMusteri2.Visible = false;
            spHande = new SoundPlayer(Resources.hande);
            spKizan = new SoundPlayer(Resources.kizan);

            
            // ilk değerleri initialize ediyoruz. sorun olursa exception u bastırıyoruz.
            try
            {
                pbUcak2.Image.RotateFlip(RotateFlipType.Rotate180FlipX);
                havaalani = new Havaalani();
                simulasyon = new Simulasyon(havaalani);
                simulasyon.BaslangicDurumunaGetir();
                havaalani.Zaman = DateTime.Now;
                timer1.Start();
                timerZaman.Start();

                // hangarları da random olarak dolu veya boş olarak oluşturuyoruz.
                // form ile ilgili bir işlem olduğundan bunu simulasyon sınıfının içinde yapmadık.
                int i = 0;
                foreach (HangarPictureBox hpb in pnlHangar.Controls.OfType<HangarPictureBox>())
                {
                    
                    if (rnd.Next(1, 10) >= 5)
                    {
                        havaalani.Hangarlar[i] = new Hangar();
                        havaalani.Hangarlar[i].Ucak = new Ucak();
                        // havaalani.Hangarlar[i].Ucak.Pilot = ((Pilot)simulasyon.rastgeleKisiOlustur(KisiTipi.Personel)); // TODO: castteki sorunu düzelt

                        havaalani.Hangarlar[i].Ucak.Pilot = new Pilot(simulasyon.rastgeleKisiOlustur(KisiTipi.Personel));
                        hpb.HangariDoluGoster();
                    }
                    else
                    {
                        hpb.HangariBosGoster();
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir sorun oluştu: " + ex.Message);
                throw;
            }


            // uçuş listesini listeyen panoyu güncelliyoruz. yine form tabanlı işlemler...
            PanoyuGuncelle();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // ışığı yanıp söndürmek için 
            // timer içinde kırmızı ise yeşile, yeşil ise kırmızıya çeviriyoruz.
            if (pbYesilIsik1.Visible)
            {
                pbKirmiziIsik1.Visible = true;
                pbKirmiziIsik1.Visible = true;
                pbYesilIsik1.Visible = false;
                pbYesilIsik2.Visible = false;
            }
            else {
                pbYesilIsik1.Visible = true;
                pbYesilIsik2.Visible = true;
                pbKirmiziIsik1.Visible = false;
                pbKirmiziIsik1.Visible = false;
            }
        }

        private void timerUcakInis_Tick(object sender, EventArgs e)
        {
            if (pbUcak.Location.Y >= 260)
            {
                pbUcak.Location = new Point(pbUcak.Location.X + 1, pbUcak.Location.Y - 6);
                pbUcak.Size = new Size(pbUcak.Size.Width - 2, pbUcak.Size.Height - 2);

            }
            else if (pbUcak.Location.Y >= 200)
            {
                pbUcak.Location = new Point(pbUcak.Location.X, pbUcak.Location.Y - 6);
            }
            else {
                ucakInisAnimasyonuOynatiliyor = false;
                timerUcakInis.Stop();

                pbYolcular = new List<PictureBox>();
                timerYolcuInis.Start();
            }
            
        }

        private void ucakIndir(int indirilecekYolcuSayisi) {

            if (!spKizanOynatildiMi) {
                spKizan.PlaySync();
                spKizanOynatildiMi = true;
            }
            
            // yolcu sayısı 5 ten fazlaysa sadece 5 ini animasyonda göster
            if (indirilecekYolcuSayisi >= 5) indirilecekYolcuSayisi = 5; 
            lbOlaylar.Items.Add(ZamaniGelenUcus.ToString() + " iniyor");
            lbOlaylar.SelectedIndex = lbOlaylar.Items.Count - 1;
            // gelen uçağı rastgele olarak oluşturuyoruz.
            inenUcak = new Ucak();
            inenUcak.Pilot = new Pilot(simulasyon.rastgeleKisiOlustur(KisiTipi.Personel));
            
            this.indirilecekYolcuSayisi = indirilecekYolcuSayisi;
            pbUcak.Location = new Point(205, 560);
            pbUcak.Size = new System.Drawing.Size(172, 172);
            ucakInisAnimasyonuOynatiliyor = true;
            timerUcakInis.Start();
            timerZaman.Stop();            
        }

        private void timerYolcuInis_Tick(object sender, EventArgs e)
        {
            
            
            if ((pbYolcular.Count < 1 || pbYolcular.Last<PictureBox>().Location.X < 160)
                && pbYolcular.Count < indirilecekYolcuSayisi) 
            {
                
                PictureBox yolcu = new PictureBox();
                yolcu.Location = new Point(208, 208);
                yolcu.Size = new System.Drawing.Size(42, 61);
                rand = rnd.Next(1, 5);
                yolcu.Image = Image.FromFile("resimler\\eleman" + rand.ToString() + ".png");
                yolcu.BackColor = Color.Transparent;

                yolcu.Image.RotateFlip(RotateFlipType.Rotate180FlipY);
                pbYolcular.Add(yolcu);
                yolcu.Show();

                this.Controls.Add(yolcu);
  
            }
            else { 
                foreach (PictureBox y in pbYolcular){
                    y.Location = new Point(y.Location.X - 5, y.Location.Y);
                }
            }

            if (pbYolcular.Count == indirilecekYolcuSayisi &&
                pbYolcular.Last<PictureBox>().Location.X < 0)
            {
                foreach (PictureBox y in pbYolcular)
                {
                    y.Dispose();
                }
                pbYolcular = null;
                timerYolcuInis.Stop();
            }

            if (pbYolcular != null && pbYolcular.Count == indirilecekYolcuSayisi)
            {
                timerUcakInis2.Start();
            }

            if (pbYolcular == null) {
                lbOlaylar.Items.Add(ZamaniGelenUcus.ToString() + "yolcularını indiriyor.");
                lbOlaylar.SelectedIndex = lbOlaylar.Items.Count - 1;
            
                timerYolcuInis.Stop();           
            }
        }

        private void timerUcakInis2_Tick(object sender, EventArgs e)
        {
            if (pbUcak.Location.Y > -120)
            {
                pbUcak.Location = new Point(pbUcak.Location.X, pbUcak.Location.Y - 6);
            }
            else {
                timerUcakInis2.Stop();
                InisAnimasyonuTamamlandi();
            }
        }

        private void InisAnimasyonuTamamlandi()
        {
            if (inenUcak != null)
            {
                havaalani.InenUcagiHangaraYerlestir(inenUcak);
            }
            else 
            {
                MessageBox.Show(this, "Uçak inerken sorun oluştu", "Hata oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            foreach (HangarPictureBox pb in pnlHangar.Controls.OfType<HangarPictureBox>())
            {
                if (!pb.DoluMu)
                {
                    pb.Image = Resources.hangar2;
                    break;
                }
            }

            timerZaman.Start();
        }


        private void ucakKaldir(int bindirilecekYolcuSayisi)
        {
            
            // bindirilecek yolcu sayısı 5 ten fazlaysa sadece 5 tane gösteriyoruz, animasyon uzun sürmesin diye..
            if (bindirilecekYolcuSayisi >= 5) bindirilecekYolcuSayisi = 5;
            lbOlaylar.Items.Add(ZamaniGelenUcus.ToString() + " kalkıyor.");
            lbOlaylar.SelectedIndex = lbOlaylar.Items.Count - 1;

            kalkanUcak = havaalani.HangardanMusaitUcakVer();
            hangardanUcakSil();

            this.bindirilecekYolcuSayisi = bindirilecekYolcuSayisi;
            pbUcak2.Location = new Point(256, -40);
            pbUcak2.Size = new System.Drawing.Size(72, 72);
            ucakKalkisAnimasyonuOynatiliyor = true;
            timerUcakKalkis.Start();
            timerZaman.Stop();
        }

        private void timerUcakKalkis_Tick(object sender, EventArgs e)
        {
            
            if (pbUcak2.Location.Y <= 200)
            {
                pbUcak2.Location = new Point(pbUcak2.Location.X, pbUcak2.Location.Y + 6);
            }
            else {
                lbOlaylar.Items.Add(ZamaniGelenUcus.ToString() + " yolculari ucaklarina biniyor.");
                lbOlaylar.SelectedIndex = lbOlaylar.Items.Count - 1;
                timerUcakKalkis.Stop();
                pbYolcular2 = new List<PictureBox>();
                if (!spHandeOynatildiMi)
                {
                    spHandeOynatildiMi = true;
                    spHande.Play();
                }
                timerYolcuBinis.Start();
            }
        }

        private void hangardanUcakSil()
        {
            foreach (HangarPictureBox pb in pnlHangar.Controls.OfType<HangarPictureBox>())
            {
                if (pb.DoluMu)
                {
                    pb.Image = Resources.hangar;
                    break;
                }
            }
        }

        private void timerYolcuBinis_Tick(object sender, EventArgs e)
        {
            if ((pbYolcular2.Count < 1 || pbYolcular2.Last<PictureBox>().Location.X > 45)
                && pbYolcular2.Count < bindirilecekYolcuSayisi)
            {

                PictureBox yolcu = new PictureBox();
                yolcu.Location = new Point(0, 208);
                yolcu.Size = new System.Drawing.Size(42, 61);
                rand = rnd.Next(1, 5);
                yolcu.Image = Image.FromFile("resimler\\eleman" + rand.ToString() + ".png");
                yolcu.BackColor = Color.Transparent;

                pbYolcular2.Add(yolcu);
                yolcu.Show();
                this.Controls.Add(yolcu);

            }
            else
            {
                foreach (PictureBox y in pbYolcular2)
                {
                    y.Location = new Point(y.Location.X + 5, y.Location.Y);
                    if (y.Location.X > 280) y.Hide();
                }
            }

            if (pbYolcular2.Count == bindirilecekYolcuSayisi &&
                pbYolcular2.Last<PictureBox>().Location.X > 256)
            {
                foreach (PictureBox y in pbYolcular2)
                {
                    y.Dispose();
                }
                pbYolcular2 = null;
                timerYolcuBinis.Stop();
                
                timerUcakKalkis2.Start();
            }

            if (pbYolcular2 == null)
            {
                timerYolcuBinis.Stop();
            }
        }

        private void timerUcakKalkis2_Tick(object sender, EventArgs e)
        {
            if (pbUcak2.Location.Y < 660)
            {
                pbUcak2.Location = new Point(pbUcak2.Location.X, pbUcak2.Location.Y + 6);
            }
            else
            {
                ucakKalkisAnimasyonuOynatiliyor = false;
                timerUcakKalkis2.Stop();
                KalkisAnimasyonuTamamlandi();
            }
        }

        private void KalkisAnimasyonuTamamlandi()
        {
            timerZaman.Start();
        }

        private void timerZaman_Tick(object sender, EventArgs e)
        {
            havaalani.Zaman = havaalani.Zaman.AddMinutes(rnd.Next(2,5));
            lbTarihSaat.Text = havaalani.Zaman.ToShortDateString() + " " + havaalani.Zaman.ToShortTimeString();
            if ((ZamaniGelenUcus = havaalani.UcuslariKontrolEt()) != null) {
                
                UcusAnimasyonuBaslat(ZamaniGelenUcus);
            }

            if (rnd.Next(0, 1000) <= 10) {
                BiletAlmaIsleminiBaslat();
            }
        }

        private void BiletAlmaIsleminiBaslat() {            
            if (biletAlmaAnimasyonuOynatiliyor) return;
            Yolcu y = new Yolcu(simulasyon.rastgeleKisiOlustur(KisiTipi.Yolcu));
            lbOlaylar.Items.Add(y.Ad + " " + y.Soyad + " bilet alıyor");
            lbOlaylar.SelectedIndex = lbOlaylar.Items.Count - 1; 
            biletAlmaAnimasyonuOynatiliyor = true;
            musteriBekle = 0;
            pbMusteri.Location = new Point(-50, 15);
            pbMusteri2.Location = new Point(30, 15);
            pbMusteri2.Visible = false;
            timerMusteri1.Start();
        }

        private void UcusAnimasyonuBaslat(Ucus ZamaniGelenUcus)
        {
            UcusButton button = (from UcusButton b in flpPano.Controls.OfType<UcusButton>()
                                 where b.ucus == ZamaniGelenUcus
                                 select b).FirstOrDefault<UcusButton>();
            button.RenkDegisimiTamamlandi += new EventHandler(UcusButton_RenkDegisimiTamamlandi);
            havaalani.Ucuslar.Remove(button.ucus);


            if (button.ucus.ucusTipi == UcusTipi.Inis)
            {
                ucakIndir(button.ucus.Yolcular.Count);
            }
            else
            {
                ucakKaldir(button.ucus.Yolcular.Count);
            }
            button.RenkDegistir(button.BackColor, Color.Green);    
        }

        private void PanoyuGuncelle() {
            UcusButton button;
            var siraliucuslar = from Ucus u in havaalani.Ucuslar
                                orderby u.KalkisZamani ascending
                                select u;
            foreach (Ucus ucus in siraliucuslar)
            {
                button = new UcusButton();
                button.Size = new System.Drawing.Size(160, 40);
                button.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
                button.Text = ucus.ToString();
                button.ucus = ucus;
                button.Click += new EventHandler(UcusButton_Click);
                flpPano.Controls.Add(button);
            }
        }

        void UcusButton_Click(object sender, EventArgs e)
        {
            ZamaniGelenUcus = (sender as UcusButton).ucus;
            UcusAnimasyonuBaslat((sender as UcusButton).ucus);
          
        }

        void UcusButton_RenkDegisimiTamamlandi(object sender, EventArgs e)
        {
            flpPano.Controls.Remove((sender) as UcusButton);
        }

        private void timerMusteri1_Tick(object sender, EventArgs e)
        {
            if (pbMusteri.Location.X < 30)
            {
                pbMusteri.Location = new Point(pbMusteri.Location.X + 1, pbMusteri.Location.Y);
            }
            else {
                timerMusteri1.Stop();
                timerMusteri2.Start();
            }
        }

        private void timerMusteri2_Tick(object sender, EventArgs e)
        {
            bool biletalindi = false;
            int islemSuresi = rnd.Next(10, 40);

            if (musteriBekle < islemSuresi)
            {
                musteriBekle++;
            }
            else {
                if (!biletalindi) YolcuyaBiletiVer();
                if (pbMusteri.Location.X != -50 || pbMusteri.Location.Y != 30)
                {
                    pbMusteri.Location = new Point(-50, 30);
                }
                if (!pbMusteri2.Visible) pbMusteri2.Visible = true;
                pbMusteri2.Location = new Point(pbMusteri2.Location.X - 1 , pbMusteri2.Location.Y);

                if (pbMusteri2.Location.X <= -50) {
                    pbMusteri2.Visible = false;
                    pbMusteri2.Location = new Point(30, 15);
                    timerMusteri2.Stop();
                    musteriBiletAlmaAnimasyonuTamamlandi();
                }
            }
        }

        private void YolcuyaBiletiVer()
        {
            Yolcu yolcu = simulasyon.rastgeleKisiOlustur(KisiTipi.Yolcu) as Yolcu;
            // en az yolcu olan 5 uçuştan rastgele birine yerleştiriyoruz.

            List<Ucus> enAzYolcusuOlanUcuslar = (from Ucus u in havaalani.Ucuslar
                                              orderby u.Yolcular.Count ascending
                                              select u).ToList<Ucus>();

            Ucus ucus = enAzYolcusuOlanUcuslar[rnd.Next(0,enAzYolcusuOlanUcuslar.Count - 1)];
            // yolcuyu rastgele seçtiğimiz uçuşa bağlıyalım
            yolcu.Ucus = ucus;
            // rastgele bir gise personelinden biletini alsın 
            Bilet bilet = havaalani.GisePersonelleri[rnd.Next(0, havaalani.GisePersonelleri.Count - 1)]
                .BiletVer(yolcu, ucus);
            // TODO: bileti de bir yere kaydedebiliriz belki, boşuna oluşturmayalım =/

            // yolcuyu ucuşa kaydedelim
            ucus.Yolcular.Add(yolcu);
            
        }

        private void musteriBiletAlmaAnimasyonuTamamlandi()
        {
            biletAlmaAnimasyonuOynatiliyor = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BiletAlmaIsleminiBaslat();
        }



        public object ImageBox { get; set; }

        private void hangarPictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show((sender as HangarPictureBox).DoluMu.ToString());
        }

    }
}
