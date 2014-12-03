using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;

namespace PicturePerfect
{
    public partial class blend : Form
    {
        Image<Bgr, byte> original = new Image<Bgr, byte>(Properties.Resources.test_image); //source image 
        Image<Bgr, byte> add = new Image<Bgr, byte>(Properties.Resources.B); //small add image
        Image<Bgr, byte> selectedImage; //image of the selected region.Size ~ add[image]
        public MatrixSolver solver;
        public Thread blendingThread;

        //--------------------Initialise the values for mask , selectionArea and SelectionBorder
        public int[,] mask;
        public ArrayList selectionBorder;
        public ArrayList selectionArea;

        string OutFileLocation = "C:\\pics";

        //--------------Check these whats their use ???? ---------------------------
        int dx, dy;
        int xMin, xMax, yMin, yMax;



        ////----------------------------------Methods-----------------------------------
        public blend()
        {
            //-------initialise values------------------------
            InitializeComponent();
            selectionArea= new ArrayList();
            selectionBorder = new ArrayList();
            mask = new int[original.Width,original.Height];
            selectedImage = new Image<Bgr, byte>(add.Width, add.Height);
             int i, j;
		    for (int x = 0; x < original.Width; x++) 
            {
			    for (int y = 0; y < original.Height; y++)
				    mask[x,y] = 0;
		    }

            //----------------------create selectionArea list
            for (i = 0; i < original.Height; i++)
            {
                for (j = 0; j < original.Width; j++)
                {
                    //do error handling for other non-target white pixels in img
                    if ((original.Data[i,j, 0] == 255) && (original.Data[i,j, 1] == 255) &&(original.Data[i,j,2]==255))
                    {
                        Point p=new Point(j,i);
                        selectionArea.Add(p);
                    }
                }
            }

            //--------------Create selectionBorder list using laplacian[contour detection]
            Image<Gray, Byte> input = new Image<Gray, Byte>(original.Width, original.Height);
            CvInvoke.cvCvtColor(original, input, COLOR_CONVERSION.BGR2GRAY);
            Image<Gray, Byte> cont = new Image<Gray, Byte>(original.Width, original.Height);
            cont = noise(laplace(input));
            cont.Save(OutFileLocation + "\\" + "lap" + ".jpg");
            foreach (Point p in selectionArea)
            {
                cont.Data[p.Y, p.X, 0] = 255;
                if((p.X > 0)&&(p.X< original.Width)&&(p.Y > 0)&&(p.Y<original.Height))
                {
                    if ((original.Data[(p.Y), (p.X)+1, 0]!=255)||(original.Data[(p.Y), (p.X)-1, 0]!=255)||(original.Data[(p.Y)+1, (p.X), 0]!=255)||(original.Data[(p.Y)-1, (p.X), 0]!=255))
                    {
                        selectionBorder.Add(p);
                        input.Data[p.Y,p.X,0]=0;
                    }
                }
            }
            cont.Save(OutFileLocation + "\\" + "lap" + ".jpg");
            input.Save(OutFileLocation + "\\" + "incorrect_input" + ".jpg");
            updateMask();
			solver = new MatrixSolver(mask, selectionArea, original, selectedImage,xMin, yMin, input.Width, input.Height, true);
			IterationBlender blender = new IterationBlender();
			blendingThread = new Thread(new );
			blendingThread.Start();

    
    }

        //---------------Laplacian for contour-----------------------------------
        private Image<Gray, Byte> noise(Image<Gray, Byte> output1)
        {
            byte high = output1.Data[0, 0, 0];
            int i, j;
            for (i = 0; i < output1.Height; i++)
            {
                for (j = 0; j < output1.Width; j++)
                {
                    if (output1.Data[i, j, 0] > high)
                        high = output1.Data[i, j, 0];
                }
            }
            byte low = output1.Data[0, 0, 0];

            for (i = 0; i < output1.Height; i++)
            {
                for (j = 0; j < output1.Width; j++)
                {
                    if (output1.Data[i, j, 0] < low)
                        low = output1.Data[i, j, 0];
                }
            }
            double p, q;
            for (int l = 0; l < output1.Height; l++)
            {
                for (int k = 0; k < output1.Width; k++)
                {
                    p = (output1.Data[l, k, 0] - low) * 255;
                    q = (high - low);
                    output1.Data[l, k, 0] = (byte)(p / q);
                }
            }
            return output1;
        }

        private Image<Gray, Byte> laplace(Image<Gray, Byte> input)
        {
            Image<Gray, Byte> output1 = new Image<Gray, Byte>(input.Size);
            Image<Gray, Byte> input1 = new Image<Gray, Byte>(input.Width + 2, input.Height + 2);
            int[,] laplacian = new int[3, 3] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
            int i, j;
            for (i = 0; i < input.Height; i++)
            {
                for (j = 0; j < input.Width; j++)
                {
                    input1.Data[i + 1, j + 1, 0] = input.Data[i, j, 0];
                }
            }
            int ycount1;
            int xcount1 = 1;
            double sum;
            int x, y;
            while (xcount1 < input1.Height - 1)
            {
                ycount1 = 1;
                while (ycount1 < input1.Width - 1)
                {
                    sum = 0;
                    for (x = -1; x < 2; x++)
                    {
                        for (y = -1; y < 2; y++)
                        {
                            sum = (double)(sum + (input1.Data[xcount1 + x, ycount1 + y, 0] * laplacian[x + 1, y + 1]));
                        }
                    }
                    if (sum > 255)
                        sum = 255;
                    if (sum < 0)
                        sum = 0;
                    output1.Data[xcount1 - 1, ycount1 - 1, 0] = (byte)sum;
                    ycount1++;
                }
                xcount1++;
            }
            return output1;
        }
        void updateMask()
        {
            for (int x = 0; x < original.Height; x++)
            {
                for (int y = 0; y < original.Width; y++)
                    mask[x,y] = -2;
            }
            int c = 0;
            foreach (Point p in selectionArea)
            {
                    mask[p.X,p.Y]=c;
                    c++;
            }
            foreach (Point p in selectionBorder)
            {
                mask[p.X,p.Y]=-1;
            }
         }



            /*
            //Clip the motion to the display window
            if (xMin + dx < 1)
                dx = 1 - xMin;
            if (xMax + dx > Width - 1)
                dx = Width - 1 - xMax;
            if (yMin + dy < 1)
                dy = 1 - yMin;
            if (yMax + dy > Height - 1)
                dy = Height - 1 - yMax;
            //Now update the mask
            for (int x = 0; x < original.Width; x++)
            {
                for (int y = 0; y < original.Height; y++)
                    mask[x,y] = -2;
            }
            foreach (Point p in selectionBorder)
            {

                int x = p.X+dx;
                int y = p.Y+dy;
                p.X = x;
                p.Y = y;
                mask[x,y] = -1;
            }
            int c = 0;
            foreach (Point p in selectionArea)
            {
                int x = p.X + dx;
                int y = p.Y + dy;
                p.X = x;
                p.Y = y;
                mask[x,y] = c;
                c++;
            }
            xMin += dx; xMax += dx;
            yMin += dy; yMax += dy;
            dx = 0;
            dy = 0;*/


        // ----- next iteration
        public void nextIteration()
        {
		for (int i = 0; i < 100; i++)
			//solver.nextIteration();
		    //synchronized(selectedImage) 
            {
			    //solver.updateImage(selectedImage);
		    }	
	    }
        class IterationBlender
        {
		public void run() {
			int iteration = 0;
			double error;
			double Norm = 1.0;
			do {
				error = solver.getError();
				if (iteration == 1)
					Norm = Math.log(error);
				if (iteration >= 1) {
					double progress = 1.0 - Math.log(error) / Norm;
					progressBar.setValue((int)(progress*100));
					progressBar.repaint();
				}
				iteration++;
				nextIteration();
			}
			while (error > 1.0);
			return ;
		}
	}
        
    }
}

/*
for (i = 0; i < original.Height; i++)
{
    for (j = 0; j < original.Width; j++)
    {
        if (input.Data[i, j, 0] == 0)
        {
            if (input.Data[i, j, 0] == 0)
            {
                if ((input.Data[i - 1, j, 0] == 255) || (input.Data[i + 1, j, 0] == 255) || (input.Data[i, j - 1, 0] == 255) || (input.Data[i, j + 1, 0] == 255))
                    input.Data[i, j, 0] = 255;
            }
        }
    }
}
input.Save(OutFileLocation + "\\" + "correct_input" + ".jpg");
selectionArea.Clear();
for (i = 0; i < input.Height; i++)
{
    for (j = 0; j < input.Width; j++)
    {
        //do error handling for other non-target white pixels in img
        if ((input.Data[i, j, 0] == 255))
        {
            Point p = new Point(j, i);
            selectionArea.Add(p);
        }
    }
}
            

selectionBorder.Clear();
Image<Gray, Byte> output = new Image<Gray, Byte>(original.Width, original.Height);
foreach (Point p in selectionArea)
{
    cont.Data[p.Y, p.X, 0] = 255;
    if ((p.X > 0) && (p.X < input.Width) && (p.Y > 0) && (p.Y < input.Height))
    {
        if ((input.Data[(p.Y), (p.X) + 1, 0] != 255) || (input.Data[(p.Y), (p.X) - 1, 0] != 255) || (input.Data[(p.Y) + 1, (p.X), 0] != 255) || (input.Data[(p.Y) - 1, (p.X), 0] != 255))
        {
            selectionBorder.Add(p);
            output.Data[p.Y, p.X, 0] = 255;
        }
    }
}
output.Save(OutFileLocation + "\\" + "with_border" + ".jpg");
*/