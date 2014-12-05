using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Text;


using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
namespace PicturePerfect
{
    public class MatrixSolver
    {
        public ArrayList selectionArea;
        public int xMin, yMin;


        int N;
        int[] D;
        int[,] R;
       
        double[,] X;
        double[,] b;


        public MatrixSolver(int[,] mask, ArrayList selectionArea, Image<Bgr, Byte> image, Image<Gray, Byte> selectImage, int xMin, int yMin, int Width, int Height, bool flatten)
        {
            this.selectionArea = selectionArea;
            this.xMin = xMin;
            this.yMin = yMin;

            int selWidth = selectImage.Width, selHeight = selectImage.Height;
            bool[,] gradMask = new bool[selWidth, selHeight];

            for (int i = 0; i < selWidth; i++)
            {
                for (int j = 0; j < selHeight; j++)
                {
                    gradMask[i, j] = true;
                }
            }
            N = selectionArea.Count;
            D = new int[N];
            R = new int[N, 4];
            X = new double[N, 3];//For the 3 color channels
            b = new double[N, 3];
            
            int[,] dP = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

            int Np = 0;
            int x, y, selX, selY;
            int m = 0;
            //System.Console.WriteLine("xmin IS " + xMin + " ymin is " + yMin);
            foreach (Point p in selectionArea)
            {

                x = p.Y;
                y = p.X;

                selX = x - xMin;
                selY = y - yMin;

                System.Console.WriteLine("X IS " + x + " y is " + y + " selX IS " + selX + " sely is " + selY);
                Image<Bgr, Byte> selectedImage = new Image<Bgr, Byte>(selectImage.Width, selectImage.Height);
                CvInvoke.cvCvtColor(selectImage, selectedImage, COLOR_CONVERSION.GRAY2BGR);

                int pValueR=0, pValueG=0, pValueB=0;
                if ((selX > 0) && (selY > 0) && (selX < selectedImage.Width) && (selY < selectedImage.Height))
                {
                    
                pValueR = selectedImage.Data[selY, selX, 2];
                pValueG = selectedImage.Data[selY, selX, 1];
                pValueB = selectedImage.Data[selY, selX, 0];
                }

                b[m, 0] = 0.0;
                b[m, 1] = 0.0;
                b[m, 2] = 0.0;
                double weight=0;
                  if((selX>0)&&(selY>0)&& (selX < selectedImage.Width) && (selY < selectedImage.Height))
                weight = gradMask[selX, selY] ? 1.0 : 0.0;
                //System.Console.WriteLine("length is" + dP.Length);
                for (int k = 0; k < 4; k++)
                {
                    int x2 = x + dP[k, 0];
                    int y2 = y + dP[k, 1];

                    //System.Console.WriteLine("x2 is" + x2 + " and y2 is" + y2);
                    R[m, k] = -1;
                    if (x2 < 1 || x2 >= Width - 1 || y2 < 1 || y2 >= Height - 1)
                        continue;
                    Np++;
                    int index = mask[x2, y2];
                    //System.Console.WriteLine("index is " + index);
                    if (index == -1)
                    {
                        //It's a border pixel

                        b[m, 2] += image.Data[y, x, 2];
                        b[m, 1] += image.Data[y, x, 1];
                        b[m, 0] += image.Data[y, x, 0];
                    }
                    else if (index != -2)
                    {
                        R[m, k] = index;
                        selX = x2 - xMin;
                        selY = y2 - yMin;
                        System.Console.WriteLine("xmin IS " + selX + " ymin is " + selY);
                        int qValueR = selectedImage.Data[selX, selY, 2];
                        int qValueG = selectedImage.Data[selX, selY, 1];
                        int qValueB = selectedImage.Data[selX, selY, 0];

                        b[m, 2] += weight * (pValueR - qValueR);
                        b[m, 1] += weight * (pValueG - qValueG);
                        b[m, 0] += weight * (pValueB - qValueB);
                    }
                }
                D[m] = Np;
                m++;
            }

        }
        public void nextIteration()
        {
            double[,] nextX = new double[N, 3];
            for (int i = 0; i < N; i++)
            {
                for (int k = 0; k < 3; k++)
                    nextX[i, k] = b[i, k];
                for (int n = 0; n < 4; n++)
                {
                    if (R[i, n] >= 0)
                    {
                        int index = R[i, n];
                        for (int k = 0; k < 3; k++)
                            nextX[i, k] += X[index, k];
                    }
                }
                for (int k = 0; k < 3; k++)
                    nextX[i, k] /= (double)D[i];
            }
            for (int i = 0; i < N; i++)
            {
                X[i, 0] = nextX[i, 0];
                X[i, 1] = nextX[i, 1];
                X[i, 2] = nextX[i, 2];
            }
        }

        public double getError()
        {
            double total = 0.0;
            for (int i = 0; i < N; i++)
            {
                double[] error = { b[i, 0], b[i, 1], b[i, 2] };
                for (int n = 0; n < 4; n++)
                {
                    if (R[i, n] >= 0)
                    {
                        int index = R[i, n];
                        for (int k = 0; k < 3; k++)
                            error[k] += X[index, k];
                    }
                }
                error[0] -= D[i] * X[i, 0];
                error[1] -= D[i] * X[i, 1];
                error[2] -= D[i] * X[i, 2];
                total += (error[0] * error[0] + error[1] * error[1] + error[2] * error[2]);
            }
            return Math.Sqrt(total);
        }

        public void updateImage(Image<Gray, byte> selectedImage)
        {
            int x, y, i = 0;

            foreach (Point p in selectionArea)
            {

                x = p.X - xMin;
                y = p.Y - yMin;
                int R = (int)Math.Round(X[i, 0]);
                int G = (int)Math.Round(X[i, 1]);
                int B = (int)Math.Round(X[i, 2]);
                if (R > 255) R = 255;
                if (R < 0) R = 0;
                if (G > 255) G = 255;
                if (G < 0) G = 0;
                if (B > 255) B = 255;
                if (B < 0) B = 0;
                int RGB = (int)(0xFF000000 | (R << 16) & 0xFF0000 | (G << 8) & 0xFF00 | B & 0xFF);
                selectedImage.Data[x, y, 0] = (byte)RGB;
                i++;
            }
        }
    }
}









