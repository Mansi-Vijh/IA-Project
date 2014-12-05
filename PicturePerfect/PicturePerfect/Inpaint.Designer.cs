namespace PicturePerfect
{
    partial class Inpaint
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.imgBoxlbp = new Emgu.CV.UI.ImageBox();
            this.imgBox1_bin = new Emgu.CV.UI.ImageBox();
            this.imgBoxgray = new Emgu.CV.UI.ImageBox();
            this.imgBoxIP = new Emgu.CV.UI.ImageBox();
            this.imgTest = new Emgu.CV.UI.ImageBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.imgBoxlbp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgBox1_bin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgBoxgray)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgBoxIP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgTest)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(177, 360);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(177, 426);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // imgBoxlbp
            // 
            this.imgBoxlbp.Location = new System.Drawing.Point(488, 333);
            this.imgBoxlbp.Name = "imgBoxlbp";
            this.imgBoxlbp.Size = new System.Drawing.Size(300, 273);
            this.imgBoxlbp.TabIndex = 6;
            this.imgBoxlbp.TabStop = false;
            // 
            // imgBox1_bin
            // 
            this.imgBox1_bin.Location = new System.Drawing.Point(488, 26);
            this.imgBox1_bin.Name = "imgBox1_bin";
            this.imgBox1_bin.Size = new System.Drawing.Size(300, 273);
            this.imgBox1_bin.TabIndex = 5;
            this.imgBox1_bin.TabStop = false;
            // 
            // imgBoxgray
            // 
            this.imgBoxgray.Location = new System.Drawing.Point(826, 26);
            this.imgBoxgray.Name = "imgBoxgray";
            this.imgBoxgray.Size = new System.Drawing.Size(300, 273);
            this.imgBoxgray.TabIndex = 4;
            this.imgBoxgray.TabStop = false;
            // 
            // imgBoxIP
            // 
            this.imgBoxIP.Location = new System.Drawing.Point(826, 333);
            this.imgBoxIP.Name = "imgBoxIP";
            this.imgBoxIP.Size = new System.Drawing.Size(300, 273);
            this.imgBoxIP.TabIndex = 3;
            this.imgBoxIP.TabStop = false;
            // 
            // imgTest
            // 
            this.imgTest.Location = new System.Drawing.Point(123, 26);
            this.imgTest.Name = "imgTest";
            this.imgTest.Size = new System.Drawing.Size(300, 273);
            this.imgTest.TabIndex = 2;
            this.imgTest.TabStop = false;
            // 
            // Inpaint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1341, 750);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.imgBoxlbp);
            this.Controls.Add(this.imgBox1_bin);
            this.Controls.Add(this.imgBoxgray);
            this.Controls.Add(this.imgBoxIP);
            this.Controls.Add(this.imgTest);
            this.Name = "Inpaint";
            this.Text = "Inpaint";
            ((System.ComponentModel.ISupportInitialize)(this.imgBoxlbp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgBox1_bin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgBoxgray)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgBoxIP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgTest)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox imgTest;
        private Emgu.CV.UI.ImageBox imgBoxIP;
        private Emgu.CV.UI.ImageBox imgBox1_bin;
        private Emgu.CV.UI.ImageBox imgBoxlbp;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private Emgu.CV.UI.ImageBox imgBoxgray;
        private System.Windows.Forms.Timer timer1;
    }
}