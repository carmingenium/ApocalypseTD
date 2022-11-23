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
        //int tileState; // 0 empty, 1 platformed, 2 unit....
        Button[] activeMenus;
        bool Mstate; // Menu State.
                                                                                            // MAP // 
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
        } // 20x20y, 32*32pixel rectangle grid.
        private void mapGen()
        {
            // Create resources, target, natural events here.
            // editing map just after createGrid().
        }  // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 menu = new Form1();
            menu.ShowDialog();
            this.Close();
        } // return menu button.
                                                                                            // UNITS // 
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
                        activateButtons(tile, 2);
                        emptyButtonSetter(0, tile);
                        emptyButtonSetter(1, tile);
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
        private void emptyButtonSetter(int index, Tile tl)
        {
            activeMenus[index].Text = "Platform";
            activeMenus[index].Name = "activeEmpty";

            this.Controls.Add(activeMenus[index]);
            activeMenus[index].Visible = true;
            activeMenus[index].BringToFront();
            activeMenus[index].Click += (sender, EventArgs) => { emptyTileClick(sender, EventArgs, tl); };
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

                                                                                            // ENEMIES //
        private void enemySpawner()
        {
            Point topleft = tileMap[0, 0].location;
            Point topright = tileMap[19, 0].location;
            Point bottomleft = tileMap[0, 19].location;
            Point bottomright = tileMap[19, 19].location;
            // maybe create a circle (or a function that consists these 4 points and is a circle graph?)
            // i like the circle function idea.
            // circle should be a little bigger than actual map borders. (guessing 16 pixels for now.)
        }
        private void spawnerTick(object sender, EventArgs e, int wave)
        {
            // according to the wave, have a list of enemies to spawn.
            // every tick, spawn an enemy, on a random location on the circle function.
        }
        private void WaveTick(object sender, EventArgs e, int wave)
        {

        }
        private void basicMovement(object sender, EventArgs e, int speed)
        {

        }



                                                                                            // TILE // 
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
