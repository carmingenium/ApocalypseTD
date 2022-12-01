
namespace ApocalypseTD
{
    partial class PlayScene
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
            this.progressBar_wave = new System.Windows.Forms.ProgressBar();
            this.spawnTestTimer = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(858, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 22);
            this.button1.TabIndex = 0;
            this.button1.Text = "Return To Menu";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.menuReturn);
            // 
            // progressBar_wave
            // 
            this.progressBar_wave.BackColor = System.Drawing.SystemColors.ControlLight;
            this.progressBar_wave.Cursor = System.Windows.Forms.Cursors.No;
            this.progressBar_wave.ForeColor = System.Drawing.Color.Crimson;
            this.progressBar_wave.Location = new System.Drawing.Point(380, 10);
            this.progressBar_wave.Name = "progressBar_wave";
            this.progressBar_wave.Size = new System.Drawing.Size(200, 20);
            this.progressBar_wave.TabIndex = 1;
            this.progressBar_wave.Value = 40;
            // 
            // spawnTestTimer
            // 
            this.spawnTestTimer.Enabled = true;
            this.spawnTestTimer.Tick += new System.EventHandler(this.spawnTestTimer_Tick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(586, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 22);
            this.button2.TabIndex = 0;
            this.button2.Text = "Wave : X";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.skipWave);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pictureBox1.ImageLocation = "";
            this.pictureBox1.Location = new System.Drawing.Point(501, 317);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(47, 35);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // PlayScene
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 1060);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.progressBar_wave);
            this.Controls.Add(this.button1);
            this.Name = "PlayScene";
            this.Text = "Game";
            this.Load += new System.EventHandler(this.PlayScene_Load);
            this.Click += new System.EventHandler(this.skipWave);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar progressBar_wave;
        private System.Windows.Forms.Timer spawnTestTimer;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}