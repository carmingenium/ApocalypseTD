using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ApocalypseTD
{
    public partial class PlayScene : Form
    {
        Rectangle rect;
        public PlayScene()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 menu = new Form1();
            menu.ShowDialog();
            this.Close();
        }

        private void PlayScene_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen myPen = new Pen(Color.Black); //Draws the borders around the shape
            Brush myBrush = new SolidBrush(Color.Blue);//Draws the interior of the shape

            rect = new Rectangle(100, 100, 10, 10);
            g.DrawRectangle(myPen, rect);
            //e.Graphics.FillRectangle(myBrush, rect);
        }

        private void PlayScene_MouseClick(object sender, MouseEventArgs e)
        {
            Point mousePt = new Point(e.X, e.Y);
            if (rect.Contains(mousePt))
                this.Text = "You clicked on rectangle";
            else
                this.Text = "";
        }
    }
}
