﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace PicturePerfect
{
    public partial class Main : Form
    {
        string Chosen_File = "";
        public Main()
        {
            InitializeComponent();
        }

        private void browse_Click(object sender, EventArgs e)
        {
            openFD.Title = "Insert an Image";
            openFD.FileName = "";
          //  openFD.Filter = " TIFF Images|*.tif|JPEG Images|*.jpg|GIF Images|*.gif|BITMAPS|*.bmp|PNG Images|*.png|PICT Images|*.pct";
            if (openFD.ShowDialog() == DialogResult.Cancel)
            {
                MessageBox.Show("Choose image to proceed");
            }
            else
            {
                Chosen_File = openFD.FileName;
                //Image<Bgr, byte> img = new Image<Bgr, byte>(Chosen_File);
              
                Image<Bgr, byte> img = new Image<Bgr, byte>(Chosen_File);
                Original.Image = img;
            }
        }
    }
}
