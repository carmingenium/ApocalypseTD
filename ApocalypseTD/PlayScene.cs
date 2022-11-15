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
        Tile[,] tileMap;
        Rectangle[,] grid;
        Rectangle map;
        // Graphics
        Graphics g;
        Pen myPen = new Pen(Color.Black);           //Draws the borders around the shape
        Brush myBrush = new SolidBrush(Color.Blue); //Draws the interior of the shape

        // Unit state

        //int tileState; // 0 empty, 1 platformed, 2 unit....
        Button activeMenu;
        bool Mstate; // Menu State.

        public PlayScene()
        {
            InitializeComponent();
        }
        private void PlayScene_Load(object sender, EventArgs e)
        {
            grid = new Rectangle[20, 20];
            tileMap = new Tile[20, 20];
            map = new Rectangle(0, 0, 1920, 1080);
            createGrid();

            Mstate = false;
        }
        private void createGrid()
        { // 200 50 400, 400
            //int bot = map.Bottom - map.Top;
            //int right = map.Right - map.Left;
            int bot = 400;
            int right = 400;
            for (int y = 0; y < bot; y += 20)
            {
                for (int x = 0; x < right; x += 20)
                {
                    Rectangle currentRect = new Rectangle(200 + x, 50 + y, 20, 20);
                    Tile currentTile = new Tile(currentRect);
                    grid[x / 20, y / 20] = currentRect;
                    tileMap[x / 20, y / 20] = currentTile;
                }
            }
        } // 20*20, 20pixel rectangle grid.

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 menu = new Form1();
            menu.ShowDialog();
            this.Close();
        }

        private void PlayScene_Paint(object sender, PaintEventArgs e) 
            // Right now, every mouseclick that is on a different surface repaints the whole form.
            // changing container maps surface to 1080 - 1920 could fix the issue.
            // whether the draw function is on the paint event or in load / shown, repainting happens. If function is on load, it does not repaint itself and map gets removed.
            // if function is on the paint event, it repaints itself and maintains itself. So function will stay on paint for now, but painting in load is commented and conserved.
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
                            addUnit(tileMap[x,y]);
                            break;
                        }
                    }
                    break;
                }
            }
        }
        private void addUnit(Tile tile) // tp = tilepoint
        {
            if (Mstate)
            {
                this.Controls.Remove(activeMenu);
                Mstate = false;
            }
            else    // needs more control mechanisms
            {       // action changes for the state of that tile = empty, platform, unit, resource, blockage etc.
                switch (tile.State)
                {
                    case 0:
                        Button u1 = new Button();
                        u1.Text = "Platform";
                        u1.Location = new Point(tile.loc.X - 10, tile.loc.Y - 20);
                        u1.Size = new Size(40, 20);
                        u1.Name = "activeEmpty";
                        Mstate = true;
                        activeMenu = u1;

                        this.Controls.Add(activeMenu);
                        activeMenu.Visible = true;
                        activeMenu.BringToFront();
                        activeMenu.Click += emptyTileClick; // set a function for button click.
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                }
                //Button u1 = new Button();
                //u1.Text = "test";
                //u1.Location = new Point(tp.X - 10, tp.Y-20);
                //u1.Size = new Size(40, 20);
                //u1.Name = "activeEmpty";
                //Mstate = true;
                //activeMenu = u1;

                //this.Controls.Add(activeMenu);
                //activeMenu.Visible = true;
                //activeMenu.BringToFront();
                //activeMenu.Click += emptyTileClick; // set a function for button click.
            }

        }
        private void emptyTileClick(object sender, EventArgs e)
        {
            Point bLoc = new Point();
            try
            {
                // need to reach tile or rectangle here. 
                Control[] x = this.Controls.Find("activeEmpty", true);
                bLoc = x[0].Location;
            }
            catch (Exception)
            {

            }
            PictureBox pb1 = new PictureBox();
            pb1.ImageLocation = "images\\platform_20.png";
            pb1.Size = new Size(20, 20);        // 100 100 and
            pb1.Location = new Point(bLoc.X+10, bLoc.Y + 20);
            this.Controls.Add(pb1);
            pb1.BringToFront();
            this.Text = "testremove";

            this.Controls.Remove(activeMenu);
            Mstate = false;
        }
        private void platformTileClick(object sender, EventArgs e)
        {

        }
    }
    public class Tile
    {
        public Rectangle tileRect; //Drawn Rectangle of Tile
        public Point loc; // Location of tile
        public int State; // state of tile = empty, platformed, unit, resources etc...
        public int[,] id;
        public Tile(Rectangle rect)
        {
            tileRect = rect;
            loc = rect.Location;
            State = 0; // empty
        }
    }
}
