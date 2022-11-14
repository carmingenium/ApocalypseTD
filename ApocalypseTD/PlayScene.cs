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
        // Map
        Rectangle[,] grid;
        Rectangle map;
        // Graphics
        Pen myPen = new Pen(Color.Black);           //Draws the borders around the shape
        Brush myBrush = new SolidBrush(Color.Blue); //Draws the interior of the shape
        // Unit state
        bool Mstate;
        Button activeMenu;
        public PlayScene()
        {
            InitializeComponent();
        }
        private void PlayScene_Load(object sender, EventArgs e)
        {
            grid = new Rectangle[20, 20];
            map = new Rectangle(200, 50, 400, 400);
            createGrid();
            Mstate = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 menu = new Form1();
            menu.ShowDialog();
            this.Close();
        }

        private void PlayScene_Paint(object sender, PaintEventArgs e) // infinitely called rn.
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(myPen, map);
            fillGrid(g, myPen);
            //e.Graphics.FillRectangle(myBrush, map);
        }
        private void fillGrid(Graphics g, Pen p)
        {
            for (int y = 0; y < 20; y += 1)
            {
                for (int x = 0; x < 20; x += 1){
                    g.DrawRectangle(p, grid[x,y]);
                }
            }
        }
        private void createGrid()
        {
            int bot = map.Bottom - map.Top;
            int right = map.Right - map.Left;
            for (int y = 0; y < bot; y += 20)
            {
                for (int x = 0; x < right; x += 20)
                {
                    Rectangle currentRect = new Rectangle(200 + x, 50 + y, 20, 20);
                    grid[x / 20, y / 20] = currentRect;
                }
            }
        } // 20*20, 20pixel rectangle grid.

        private void PlayScene_MouseClick(object sender, MouseEventArgs e)
        {
            Point mousePt = new Point(e.X, e.Y);
            if (map.Contains(mousePt))
            {
                this.Text = "You clicked on the map!";
                clickReg(mousePt);
            }
            else this.Text = "";
        }
        private void clickReg(Point mousePt)
        {
            for (int x = 0; x < 20; x += 1)
            {
                if (mousePt.X < (grid[x, 0].X + 20))
                {
                    for (int y = 0; y < 20; y += 1)
                    {
                        if (mousePt.Y < (grid[x, y].Y + 20))
                        {
                            this.Text = "You clicked on rectangle" + x + "x and " + y + "y";
                            Point tilePoint = new Point(grid[x, y].X, grid[x, y].Y);
                            addUnit(tilePoint);
                            break;
                        }
                    }
                    break;
                }
            }
        }
        private void addUnit(Point tp) // tp = tilepoint
        {
            if (Mstate)
            {
                this.Controls.Remove(activeMenu);
                Mstate = false;
            }
            else
            {
                Button u1 = new Button();
                u1.Text = "test";
                u1.Location = new Point(tp.X - 10, tp.Y-20);
                u1.Size = new Size(40, 20);
                
                Mstate = true;
                activeMenu = u1;

                this.Controls.Add(activeMenu);
                activeMenu.Visible = true;
                activeMenu.BringToFront();
            }
        }
    }
}
