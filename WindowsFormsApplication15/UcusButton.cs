using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication15
{
    public partial class UcusButton : Button
    {
        public string UcusKodu;
        public Ucus ucus;
        public event EventHandler RenkDegisimiTamamlandi;

        protected virtual void OnRenkDegisimiTamamlandi(EventArgs args) {
            if (RenkDegisimiTamamlandi != null)
                RenkDegisimiTamamlandi(this, args);
        }

        public UcusButton()
        {
            InitializeComponent();
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        Color hedefRenk;

        public void RenkDegistir(Color KaynakRenk, Color HedefRenk){
            hedefRenk = HedefRenk;
            BackColor = KaynakRenk;
            TimerOrjinalRengeDondur.Start();
        }

        private void timerOrjinalRengeDondur_Tick(object sender, EventArgs e)
        {                        
            if (BackColor.R != hedefRenk.R || BackColor.G != hedefRenk.G || BackColor.B != hedefRenk.B)
            {
                int r = BackColor.R;
                int g = BackColor.G;
                int b = BackColor.B;

                if (r < hedefRenk.R) r++; else if (r != hedefRenk.R) r--;
                if (g < hedefRenk.G) g++; else if (g != hedefRenk.G) g--;
                if (b < hedefRenk.B) b++; else if (b != hedefRenk.B) b--;

                BackColor = Color.FromArgb(r,g,b);
                
            }
            else {
                
                TimerOrjinalRengeDondur.Stop();
                OnRenkDegisimiTamamlandi(EventArgs.Empty);
            }            
        }
    }
}
