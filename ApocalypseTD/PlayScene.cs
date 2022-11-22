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
        // Graphics
        Graphics g;
        Pen myPen = new Pen(Color.Black);           //Draws the borders around the shape
        Brush myBrush = new SolidBrush(Color.Blue); //Draws the interior of the shape
        //int tileState; // 0 empty, 1 platformed, 2 unit....
        Button[] activeMenus;
        bool Mstate; // Menu State.

        public PlayScene()
        {
            InitializeComponent();
        }
        private void PlayScene_Load(object sender, EventArgs e)
        {
            activeMenus = new Button[10];
            tileMap = new Tile[20, 20];
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
                    currentTile.tileSprite.MouseClick += (sender, EventArgs) => { addUnit(currentTile); };
                    this.Controls.Add(currentTile.tileSprite);

                    tileMap[xt,yt] = currentTile;
                    xt += 1;
                }
                xt = 0;
                yt += 1;
            }
        } // 20*20, 20pixel rectangle grid.
        private void PlayScene_Paint(object sender, PaintEventArgs e)
        {
            // Right now, every mouseclick that is on a different surface repaints the whole form.
            // changing container maps surface to 1080 - 1920 could fix the issue.
            // whether the draw function is on the paint event or in load / shown, repainting happens. If function is on load, it does not repaint itself and map gets removed.
            // if function is on the paint event, it repaints itself and maintains itself. So function will stay on paint for now, but painting in load is commented and conserved.
            // g.DrawRectangle(myPen, map);
            //e.Graphics.FillRectangle(myBrush, map);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 menu = new Form1();
            menu.ShowDialog();
            this.Close();
        } // return menu button.

        private void activateButtons(Tile tl, int times)
        {
            // need to calculate location for amount of times. also add y levels for 3 times, 3 times.
            for (int x = 0; x < times; x++)
            {
                int levelreset = (3 - times % 3) * ((x+3) / (((times+3)/3)*3)); // if times > 3, set it to 3 until x > 3 for every 3 loop. 6 3 3 , 5 3 2, 4 3 1, 7 3 3 1, 8 3 3 2, 2 = 2
                int xfix = (((3  - (levelreset) -1 ) * 16) - ( 32 * (x - ((x/3)*3)) ) );
                int yfix = ((x / 3) * 20);


                Button u1 = new Button();
                u1.Text = "Creation" + (x+1);
                u1.Location = new Point(tl.location.X-xfix, (tl.location.Y-20)-yfix);
                u1.Size = new Size(32, 20);
                u1.Name = "active" + (x+1);
                activeMenus[x] = u1;
            }
            Mstate = true;
        }
        private void deactivateButtons()
        {
            for (int ind = 0; activeMenus.Length>ind;ind++)
            {
                this.Controls.Remove(activeMenus[ind]);
            }
            activeMenus = new Button[10];
        }
        private void addUnit(Tile tile) // tp = tilepoint
        {
            if (Mstate)
            {
                deactivateButtons();
                Mstate = false;
            }
            else    // needs more control mechanisms
            {       // action changes for the state of that tile = empty, platform, unit, resource, blockage etc.
                switch (tile.State)
                {
                    case 0:
                        activateButtons(tile, 4);
                        activeMenus[0].Text = "Platform";
                        activeMenus[0].Name = "activeEmpty";

                        this.Controls.Add(activeMenus[0]);
                        activeMenus[0].Visible = true;
                        activeMenus[0].BringToFront();
                        // separator
                        activeMenus[1].Text = "Platform";
                        activeMenus[1].Name = "activeEmpty";

                        this.Controls.Add(activeMenus[1]);
                        activeMenus[1].Visible = true;
                        activeMenus[1].BringToFront();

                        activeMenus[1].Click += (sender, EventArgs) => { emptyTileClick(sender, EventArgs, tile); };
                        // separator
                        activeMenus[2].Text = "Platform";
                        activeMenus[2].Name = "activeEmpty";

                        this.Controls.Add(activeMenus[2]);
                        activeMenus[2].Visible = true;
                        activeMenus[2].BringToFront();

                        activeMenus[2].Click += (sender, EventArgs) => { emptyTileClick(sender, EventArgs, tile); };
                        // separator
                        activeMenus[3].Text = "Platform";
                        activeMenus[3].Name = "activeEmpty";

                        this.Controls.Add(activeMenus[3]);
                        activeMenus[3].Visible = true;
                        activeMenus[3].BringToFront();

                        activeMenus[3].Click += (sender, EventArgs) => { emptyTileClick(sender, EventArgs, tile); };
                        break;
                    case 1:
                        //activeMenu = activeButton(tile);



                        //activeMenu.Click += (sender, EventArgs) => { platformTileClick(sender, EventArgs, tile); };
                        break;
                    case 2:
                        break;
                }
            }

        }
        private void emptyTileClick(object sender, EventArgs e, Tile tl)
        {
            tl.State = 1;
            tl.SetSprite(tl.State);

            deactivateButtons();
            Mstate = false;
        }
        private void platformTileClick(object sender, EventArgs e, Tile tl)
        {

        }
    }
    public class Tile
    {
        // if tile state is above a certain point, should not check for add unit. unit states will fill states until that point.
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
                    tileSprite.ImageLocation = "images\\tile-platform.png";
                    // platformed sprite
                    break;
                case 2:
                    tileSprite.ImageLocation = "images\\tile-u1.png";
                    // platform + unit sprite
                    break;
            }
        }
    }
}
