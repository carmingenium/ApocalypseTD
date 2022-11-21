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
        Rectangle map;
        // Graphics
        Graphics g;
        Pen myPen = new Pen(Color.Black);           //Draws the borders around the shape
        Brush myBrush = new SolidBrush(Color.Blue); //Draws the interior of the shape
        //int tileState; // 0 empty, 1 platformed, 2 unit....
        Button activeMenu;
        bool Mstate; // Menu State.

        public PlayScene()
        {
            InitializeComponent();
        }
        private void PlayScene_Load(object sender, EventArgs e)
        {
            tileMap = new Tile[20, 20];
            map = new Rectangle(0, 0, 1920, 1080);
            createGrid();

            Mstate = false;
        }
        private void createGrid()
        {
            int bot = 640;
            int right = 640;
            int yt = 0; // y times
            int xt = 0; // x times
            for (int y = 0; y < bot; y += 32)
            {
                for (int x = 0; x < right; x += 32)
                {
                    Tile currentTile = new Tile(new Point(200 + x, 50 + y));
                    // currentTile.tileSprite.MouseClick += (sender, EventArgs) => { emptyTileClick(sender, EventArgs, currentTile); };
                    currentTile.tileSprite.MouseClick += (sender, EventArgs) => { addUnit(currentTile); };
                    this.Controls.Add(currentTile.tileSprite);
                    tileMap[xt,yt] = currentTile;
                    xt += 1;
                }
                xt = 0;
                yt += 1;
            }
        } // 20*20, 20pixel rectangle grid.
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 menu = new Form1();
            menu.ShowDialog();
            this.Close();
        } // return menu button.

        private void PlayScene_Paint(object sender, PaintEventArgs e) 
            // Right now, every mouseclick that is on a different surface repaints the whole form.
            // changing container maps surface to 1080 - 1920 could fix the issue.
            // whether the draw function is on the paint event or in load / shown, repainting happens. If function is on load, it does not repaint itself and map gets removed.
            // if function is on the paint event, it repaints itself and maintains itself. So function will stay on paint for now, but painting in load is commented and conserved.
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(myPen, map);
            //e.Graphics.FillRectangle(myBrush, map);
        }
        private Button activeButton(Tile tl)
        {
            Button u1 = new Button();
            u1.Text = "Creation";
            u1.Location = new Point(tl.location.X - 10, tl.location.Y - 20);
            u1.Size = new Size(40, 20);
            u1.Name = "active";
            Mstate = true;
            return u1;
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
                        activeMenu = activeButton(tile);
                        activeMenu.Text = "Platform";
                        activeMenu.Name = "activeEmpty";

                        this.Controls.Add(activeMenu);
                        activeMenu.Visible = true;
                        activeMenu.BringToFront();
                        
                        activeMenu.Click += (sender, EventArgs) => { emptyTileClick(sender, EventArgs, tile); };
                        //activeMenu.Click += emptyTileClick; // set a function for button click.
                        break;
                    case 1:
                        activeMenu = activeButton(tile);



                        activeMenu.Click += (sender, EventArgs) => { platformTileClick(sender, EventArgs, tile); };
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
        private void emptyTileClick(object sender, EventArgs e, Tile tl)
        {
            tl.State = 1;
            tl.SetSprite(tl.State);
            
            this.Controls.Remove(activeMenu);
            Mstate = false;
        }
        private void platformTileClick(object sender, EventArgs e, Tile tl)
        {

        }
    }
    public class Tile
    {
        public PictureBox tileSprite; // rectangle should become a picturebox? some form to hold tile picture
        public Point location; // Location of tile
        public int State; // state of tile = empty, platformed, unit, resources etc...
        public int[,] id;
        public Tile(Point locat)
        {
            location = locat;
            State = 0; // empty

            tileSprite = new PictureBox();
            tileSprite.Size = new Size(32, 32);
            tileSprite.Location = location;

            SetSprite(State);

        }

        public void SetSprite(int stateinf)
        {

            switch (stateinf)
            {
                case 0:// empty
                    tileSprite.ImageLocation = "images\\tile-empty.png";
                    break;
                case 1:
                    tileSprite.ImageLocation = "images\\tile-platformed.png";
                    // platformed sprite
                    break;
                case 2:
                    // platform + unit sprite
                    break;
            }
        }
    }
}
