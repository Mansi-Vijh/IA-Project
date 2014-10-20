namespace PicturePerfect
{
    partial class Main
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
            this.Original = new Emgu.CV.UI.ImageBox();
            this.openFD = new System.Windows.Forms.OpenFileDialog();
            this.browse = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Original)).BeginInit();
            this.SuspendLayout();
            // 
            // Original
            // 
            this.Original.Location = new System.Drawing.Point(121, 126);
            this.Original.Name = "Original";
            this.Original.Size = new System.Drawing.Size(518, 547);
            this.Original.TabIndex = 2;
            this.Original.TabStop = false;
            // 
            // openFD
            // 
            this.openFD.FileName = "openFileDialog1";
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(121, 46);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(139, 28);
            this.browse.TabIndex = 3;
            this.browse.Text = "Choose Image";
            this.browse.UseVisualStyleBackColor = true;
            this.browse.Click += new System.EventHandler(this.browse_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 750);
            this.Controls.Add(this.browse);
            this.Controls.Add(this.Original);
            this.Name = "Main";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Original)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox Original;
        private System.Windows.Forms.OpenFileDialog openFD;
        private System.Windows.Forms.Button browse;
    }
}

