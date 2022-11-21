using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApocalypseTD
{
    public partial class Form1 : Form
    {
        // Initialization.
        public Form1()
        {
            InitializeComponent();
        }

        // start function
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        // Button activation.
        private void button1_Click(object sender, EventArgs e) //Play
        {
            this.Hide();
            PlayScene ps = new PlayScene();
            ps.ShowDialog();
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e) // Options
        {
            this.Hide();
            // open options scene
            // options.showdialog
            this.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }
    }
}
