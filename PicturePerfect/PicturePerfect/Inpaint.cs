using System;
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
using Emgu.CV.UI;

namespace PicturePerfect
{
    public partial class Inpaint : Form
    {
        Image<Bgr, Byte> original = new Image<Bgr, Byte>(Properties.Resources.Penguins);
        Image<Bgr, byte> xyz; //This will store the binarized img 
        Image<Gray, Byte> Grayconv;
        //Image<Bgr, Byte> xyz = new Image<Bgr, Byte>(@"C:\Users\Saumya\Desktop\sem5\IA\standard_test_images\lena_color_256.tif");
        Image<Gray, Byte> linearbinarypattern;
        Image<Gray, Byte> obtainmask;
        public Inpaint()
        {
            InitializeComponent();
            Bitmap b_src;
            b_src = original.Bitmap;
            Bitmap bmp = new Bitmap(b_src, new Size(256, 256));

            original = new Image<Bgr, Byte>(bmp);
       
            imgTest.Image = original;
        }

        private void Inpainting_Load(object sender, EventArgs e)
        {

        }



        //--------------------------------------------- BINARIZING THE IMAGE------------------------------------------------

        public Image<Gray, Byte> Binarizingimg()
        {


            xyz = original;
            Console.Out.WriteLine("Height = " + xyz.Height);
            Console.Out.WriteLine("This is new");

            obtainmask= new Image<Gray, Byte>(xyz.Size);


            //size of the structuring element
            int SE = 3;

            for (int i = 0; i < xyz.Height; i++)
            {
                for (int j = 0; j < xyz.Width; j++)
                {
                    //If yes then pixel in obtainmask at this position is set to 255 i.e white
                    if ((xyz.Data[i, j, 0] < 80) && (xyz.Data[i, j, 1] < 80) && (xyz.Data[i, j, 2] > 220))
                    {
                        obtainmask.Data[i, j, 0] = 255;
                        // performing dilation i.e extending he mask area to nullify edge effect
                        for (int ib = i - SE; ib < i + SE; ib++)
                        {
                            for (int jb = j - SE; jb < j + SE; jb++)
                            {
                                obtainmask.Data[ib, jb, 0] = 255;
                            }
                        }
                    }
                    else
                    {
                        obtainmask.Data[i, j, 0] = 0;
                    }

                }
            }

            return obtainmask;
        }
        //------------------------------------------ binary to decimal conversion ---------------------------------------------------

        double decimalconversion(List<int> binary)
        {
            double d = 0;

            for (int i = 0; i < binary.Count; i++)
            {
                d += binary[i] * Math.Pow(2, i);
            }
            return d;

        }

        //-----------------------------------------LINEAR BINARY PATTERN-----------------------------
        public Image<Gray, Byte> LBP()
        {

            //gray conversion
            Grayconv = new Image<Gray, Byte>(xyz.Width, xyz.Height);
            CvInvoke.cvCvtColor(xyz, Grayconv, Emgu.CV.CvEnum.COLOR_CONVERSION.BGR2GRAY);
            imgBoxgray.Image = Grayconv;

             linearbinarypattern = new Image<Gray, Byte>(xyz.Width, xyz.Height);

            double[,] matrix = new double[xyz.Width, xyz.Height];
            double max = 0.0;
            int R = 3; 

            for (int i = 0; i < xyz.Height; i++)
            {
                for (int j = 0; j < xyz.Width; j++)
                {
                    matrix[j, i] = 0;

                    if ((i > R) && (j > R) && (i < (xyz.Height - R)) && (j < (xyz.Width - R)))
                    {
                        List<int> listvals = new List<int>();
                        for (int a = i - R; a < (i + R); a++)
                        {
                            for (int b = j - R; b < (j + R); b++)
                            {
                                int centrepixel;
                                int neighbourpixel;

                                centrepixel = xyz.Data[i, j, 2];
                                neighbourpixel = xyz.Data[a, b, 2];

                                if (neighbourpixel > centrepixel)
                                {
                                    listvals.Add(1);
                                }
                                else
                                {
                                    listvals.Add(0);
                                }


                            }
                        }

                        //converting list containing binary to decimal
                        double deci = decimalconversion(listvals);
                        matrix[j, i] = deci;
                        if (deci > max)
                        {
                            max = deci;
                        }
                    }

                }
            }

            // --------------------------------Normalizing LBP matrix "matrix"------------------------------------------------- 
            for (int i = 0; i < linearbinarypattern.Height; i++)
            {
                for (int j = 0; j < linearbinarypattern.Width; j++)
                {
                    double div = matrix[j, i] / max;
                    int norm = (int)(div * 255);

                    linearbinarypattern.Data[i, j, 0] = (byte)norm;
                }
            }
            return linearbinarypattern;
        }

        Image<Bgr, Byte> resultant;
        int Si = 0;

      //  ------------------------------------------------------------------- INPAINT ---------------------------------------------------------
        public void inpaint()
        {
            resultant = new Image<Bgr, Byte>(obtainmask.Size);
            resultant = original;
            Image<Gray, byte> mask = new Image<Gray, byte>(obtainmask.Size);
            mask = obtainmask;
            Image<Gray,byte>lbp_output=linearbinarypattern;
            int block = 10;
            int[,] array = new int[(2 * block + 1) * (2 * block + 1), (2 * block + 1) * (2 * block + 1)];


            for (int i = block; i < original.Height - block; i++)
                for (int j = block; j < original.Width - block; j++)
                {

                    if (mask.Data[i, j, 0] == 255)
                    {
                        int c1 = 0;
                        for (int x = i - block; x < i + block; x++)
                        {
                            for (int y = j - block; y < j + block; y++)
                            {
                                if (mask.Data[x, y, 0] == 0)////
                                {
                                    int c2=0;
                                    for (int x1 = i - block; x1 < i + block-1; x1++)
                                    {
                                         for (int y1 = j - block; y1 < j + block-1; y1++)
                                            {
                                             //   System.Console.WriteLine("Value of c1 is "+c1+" and c2 is:"+c2);
                                                if (mask.Data[x1, y1, 0] == 0)
                                                    array[c1, c2] = Math.Abs(lbp_output.Data[x, y, 0]-lbp_output.Data[x1,y1,0]);
                                                c2++;
                                            }
                                    }
                                   
                                    
                                   
                                }
                                else
                                { 
                                    //Don't deal. shoo 
                                }
                                c1++;
                                
                            }
                        }
                        int minat=0;
                        int[] min=new int[block];

                        for (int x = 0; x < block; x++)
                        {
                          for (int y = 0; y < block; y++)
                                 min[x]+=array[x,y];
                            min[x]=min[x]/block;
        
        
                        }
                    int temp=min[0];
                        int tempat=0;
                     for (int x = 0; x < block; x++)
                        {
                         if(temp>min[x])
                         {
                             temp = min[x];
                             tempat = x;
                         }

                        }
                     int c = 0;
                     for (int x = i - block; x < i + block; x++)
                     {
                         for (int y = j - block; y < j + block; y++)
                         {
                             if (c == tempat)
                             {
                                 resultant.Data[i, j, 0] = resultant.Data[x, y, 0];
                                 resultant.Data[i, j, 1] = resultant.Data[x, y, 1];
                                 resultant.Data[i, j, 2] = resultant.Data[x, y, 2];
                                 break;
                             }

                             c++;
                         }
                     }
                    }
                    else
                    {
                        //Do nothing

                    }
                    imgBoxIP.Image = resultant;

                }


        }
        System.Threading.Thread t;
        string s = "processing";

        private void button1_Click(object sender, EventArgs e)
        {
            imgBox1_bin.Image = Binarizingimg();
            imgBoxlbp.Image = LBP();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Si = 0;
            this.Text = "processing";
            t = new System.Threading.Thread(inpaint);
            t.Start();
            timer1.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            if (t.ThreadState == System.Threading.ThreadState.Running)
            t.Suspend();
             try
             {
                imgBoxIP.Image = resultant;
//                pictureBox5.Image.Save("Result-" + Si + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                Si++;
             }
             catch(Exception ex)
             {
             }
            
            if (t.ThreadState == System.Threading.ThreadState.Suspended)
            t.Resume();
            this.Text = s;
            if (s == "DONE")
            {
                return;
            }
            timer1.Enabled = true;
        }


     


    }
}
   