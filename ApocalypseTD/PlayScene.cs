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
        Rectangle[,] grid = new Rectangle[20,20];
        Rectangle map;
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

            map = new Rectangle(200, 50, 400, 400);
            g.DrawRectangle(myPen, map);
            fillGrid(g, myPen);
            //e.Graphics.FillRectangle(myBrush, map);
        }
        private void fillGrid(Graphics g, Pen p) // 20*20, 20pixel rectangle grid.
        {
            int bot = map.Bottom - map.Top;
            int right = map.Right - map.Left;
            for (int y = 0; y < bot; y += 20)
            {
                for (int x = 0; x < right; x += 20){
                    Rectangle currentRect = new Rectangle(200 + x, 50 + y, 20, 20);
                    grid[x / 20,y / 20] = currentRect;
                    g.DrawRectangle(p, currentRect);
                }
            }
        }

        private void PlayScene_MouseClick(object sender, MouseEventArgs e)
        {
            Point mousePt = new Point(e.X, e.Y);
            if (map.Contains(mousePt))
                this.Text = "You clicked on the map!";
            else if (true) // after grid is done, this if will be used to determine which rectangle has been clicked.
            {
                this.Text = "";
            }
            else this.Text = "";
        }
    }
}
