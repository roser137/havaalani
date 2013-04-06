using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WindowsFormsApplication15.Properties;
using System.Drawing;

namespace WindowsFormsApplication15
{
    public partial class HangarPictureBox : PictureBox
    {
        public bool DoluMu { get; set; }

        public HangarPictureBox()
        {   
            InitializeComponent();
            if (DoluMu)
            {
                HangariDoluGoster();
            }
            else {
                HangariBosGoster();
            }
        }

        public HangarPictureBox(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            if (DoluMu)
            {
                HangariDoluGoster();
            }
            else
            {
                HangariBosGoster();
            }
        }

        public void HangariDoluGoster() {
            Image = Resources.hangar2;
            DoluMu = true;
        }

        public void HangariBosGoster() {
            Image = Resources.hangar;
            DoluMu = false;
        }
    }
}
