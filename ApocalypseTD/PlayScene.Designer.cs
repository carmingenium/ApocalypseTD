
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
            this.wave_info = new System.Windows.Forms.TextBox();
            this.spawnTestTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(877, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Return To Menu";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar_wave
            // 
            this.progressBar_wave.BackColor = System.Drawing.SystemColors.ControlLight;
            this.progressBar_wave.Cursor = System.Windows.Forms.Cursors.No;
            this.progressBar_wave.ForeColor = System.Drawing.Color.Crimson;
            this.progressBar_wave.Location = new System.Drawing.Point(427, 11);
            this.progressBar_wave.Name = "progressBar_wave";
            this.progressBar_wave.Size = new System.Drawing.Size(198, 23);
            this.progressBar_wave.TabIndex = 1;
            this.progressBar_wave.Value = 40;
            // 
            // wave_info
            // 
            this.wave_info.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.wave_info.Location = new System.Drawing.Point(475, 40);
            this.wave_info.Name = "wave_info";
            this.wave_info.Size = new System.Drawing.Size(84, 34);
            this.wave_info.TabIndex = 2;
            this.wave_info.Text = "Wave : X";
            this.wave_info.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // spawnTestTimer
            // 
            this.spawnTestTimer.Enabled = true;
            this.spawnTestTimer.Tick += new System.EventHandler(this.spawnTestTimer_Tick);
            // 
            // PlayScene
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 800);
            this.Controls.Add(this.wave_info);
            this.Controls.Add(this.progressBar_wave);
            this.Controls.Add(this.button1);
            this.Name = "PlayScene";
            this.Text = "d";
            this.Load += new System.EventHandler(this.PlayScene_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar progressBar_wave;
        private System.Windows.Forms.TextBox wave_info;
        private System.Windows.Forms.Timer spawnTestTimer;
    }
}