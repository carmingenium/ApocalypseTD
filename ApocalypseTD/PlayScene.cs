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
        // spawner
        int offset;
        Point topleft;
        Point topright;
        Point bottomleft;
        Point bottomright;
        Point center;
        float radius;
        // random
        Random roll;
        // MAP // 
        public PlayScene()
        {
            InitializeComponent();
        }
        private void PlayScene_Load(object sender, EventArgs e)
        {
            // initialization
            // random
            roll = new Random();
            // button container
            activeMenus = new Button[10];
            // map
            tileMap = new Tile[30, 30];
            createGrid();
            mapGen();
            // menu state
            Mstate = false;

            // spawn circle
            offset = 16;
            topleft = new Point(tileMap[0, 0].location.X - offset, tileMap[0, 0].location.Y - offset);
            topright = new Point(tileMap[29, 0].location.X + 32 + offset, tileMap[29, 0].location.Y - offset);
            bottomleft = new Point(tileMap[0, 29].location.X - offset, tileMap[0, 29].location.Y + 32 + offset);
            bottomright = new Point(tileMap[29, 29].location.X + 32 + offset, tileMap[29, 29].location.Y + 32 + offset);

            center = new Point((topleft.X + topright.X) / 2, (topright.Y + bottomright.Y) / 2);
            double radiusEx = (Math.Pow((center.X - topleft.X), 2) + (Math.Pow((center.Y - topleft.Y), 2)));
            radius = (float)Math.Sqrt((radiusEx));

        }
        private void menuReturn(object sender, EventArgs e)
        {
            this.Hide();
            Form1 menu = new Form1();
            menu.ShowDialog();
            this.Close();
        }   // return menu button.
        private void createGrid()
        {
            int bot = 32*30;    // size of tile * amount of tile
            int right = 32*30;  // size of tile * amount of tile
            // 32*30 = 960 which is also 1920 / 2, so it is perfect width for me, for now.
            int yt = 0;         // y times
            int xt = 0;         // x times
            int yoffset = 40;
            for (int y = 0; y < bot; y += 32)
            {
                for (int x = 0; x < right; x += 32)
                {
                    Tile currentTile = new Tile(new Point(0 + x, yoffset + y));
                    currentTile.tileSprite.MouseClick += (sender, EventArgs) => { changeState(currentTile); };
                    this.Controls.Add(currentTile.tileSprite);

                    tileMap[xt,yt] = currentTile;
                    xt += 1;
                }
                xt = 0;
                yt += 1;
            }
        } // 30*30, 32*32pixel rectangle grid.
        private void mapGen() // RIGHT NOW THE TILES ARE NOT APPLIED.
        {
            // gen location holder
            // dont need categorization, just need the generated tiles.

            Point[] genArray = new Point[50];
            int last = 0;

            // objective
            Point obj = new Point(roll.Next(12,18), roll.Next(12, 18));
            genArray[last++] = obj;  // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! test
            tileImplement(obj, state.Target); // set tile to Target

            // corruption
            genArray[last++] = new Point(obj.X, obj.Y + 1);     // midright
            genArray[last++] = new Point(obj.X, obj.Y - 1);     // midleft
            genArray[last++] = new Point(obj.X - 1, obj.Y);     // topmid
            genArray[last++] = new Point(obj.X + 1, obj.Y);     // botmid

            genArray[last++] = new Point(obj.X - 1, obj.Y - 1); // topleft 
            genArray[last++] = new Point(obj.X - 1, obj.Y + 1); // topright     
            genArray[last++] = new Point(obj.X + 1, obj.Y - 1); // botleft    
            genArray[last++] = new Point(obj.X + 1, obj.Y + 1); // botright 

            for (int rep = 0; rep < 8; rep++) // set all corrupted tiles.
            {
                tileImplement(genArray[rep+1], state.Corrupted);
            }


            // resources
            for(int amount = 0; amount < 3; amount++)
            {
                Point resourcePoint;
                do
                {
                    resourcePoint = new Point(roll.Next(0, 30), roll.Next(0, 30));
                } while (tileGenerated(resourcePoint,genArray,last));
                genArray[last++] = resourcePoint;
                tileImplement(genArray[last - 1], state.Resource); // set tile state to resource.
            }
            
            // boulders
            int boulderAmount = roll.Next(2, 8);
            for (int amount = 0; amount < boulderAmount; amount++)
            {
                Point boulderPoint;
                do
                {
                    boulderPoint = new Point(roll.Next(0, 30), roll.Next(0, 30));
                } while (tileGenerated(boulderPoint, genArray, last));
                genArray[last++] = boulderPoint;
                tileImplement(genArray[last - 1], state.Boulder); // set tile state to boulder.
            }
        }
        private bool tileGenerated(Point current, Point[] locArray, int last)
        {
            for (int len = 0; len < (last + 1); len++)
            {
                if (current == locArray[len])
                {
                    return true;
                }
            }
            return false;
        }
        private void tileImplement(Point current, state state)
        {
            tileMap[current.X, current.Y].SetState(state);
        }

                                                                                             // UNITS // 
        private void createButtons(Tile tl, int times)
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
        private void activateButtons(Tile tl, state stateinfo)
        {
            switch (stateinfo)
            {
                case state.Empty:

                    createButtons(tl, 1);
                    activeMenus[0].Text = "Platform";
                    activeMenus[0].Name = "activeEmpty";

                    this.Controls.Add(activeMenus[0]);
                    activeMenus[0].Visible = true;
                    activeMenus[0].BringToFront();
                    activeMenus[0].Click += (sender, EventArgs) => { emptyTileClick(sender, EventArgs, tl); };
                    break;

                case state.Platformed:
                    createButtons(tl, 2);
                    for(int unit = 0; 2>unit; unit++)
                    {
                        activeMenus[unit].Text = "Unit" + (unit + 1);
                        activeMenus[unit].Name = "active";

                        this.Controls.Add(activeMenus[unit]);
                        activeMenus[unit].Visible = true;
                        activeMenus[unit].BringToFront();


                        state current = (state)(unit + 6);
                        activeMenus[unit].Click += (sender, EventArgs) => { platformTileClick(sender, EventArgs, tl, current); };
                    }
                    break;
                case state.Target:
                    break;
                case state.Corrupted:
                    break;
                case state.Resource:
                    createButtons(tl, 1);
                    activeMenus[0].Text = "Mine";
                    activeMenus[0].Name = "active";

                    this.Controls.Add(activeMenus[0]);
                    activeMenus[0].Visible = true;
                    activeMenus[0].BringToFront();
                    activeMenus[0].Click += (sender, EventArgs) => { resourceTileClick(sender, EventArgs, tl); };
                    break;
                case state.Boulder:
                    break;
            }
            if ((int)stateinfo >= 6) // if state is a unit.
            {
                //if (x == 0)
                //{
                //    activeMenus[x].Text = "Sell";
                //    // need differenet click action
                //}
                //else
                //{
                //    activeMenus[x].Text = "Upgrade";
                //    // need differenet click action
                //}

                //activeMenus[x].Name = "active";

                //this.Controls.Add(activeMenus[x]);
                //activeMenus[x].Visible = true;
                //activeMenus[x].BringToFront();
            }
        }
        private void deactivateButtons()
        {
            for (int ind = 0; activeMenus.Length>ind;ind++)
            {
                this.Controls.Remove(activeMenus[ind]);
            }
            activeMenus = new Button[10];
        }
        private void changeState(Tile tile)
        {
            if (Mstate)
            {
                deactivateButtons();
                Mstate = false;
            }
            else    // needs more control mechanisms
            {       // action changes for the state of that tile = empty, platform, unit, resource, blockage etc.
                activateButtons(tile, tile.State);
            }

        }
        private void emptyTileClick(object sender, EventArgs e, Tile tl)
        {
            tl.SetState(state.Platformed);

            deactivateButtons();
            Mstate = false;
        }
        private void platformTileClick(object sender, EventArgs e, Tile tl, state newState)
        {
            tl.SetState(newState);

            deactivateButtons();
            Mstate = false;
        }
        private void resourceTileClick(object sender, EventArgs e, Tile tl)
        {
            tl.SetState(state.Mine); 

            deactivateButtons();
            Mstate = false;
        }

        // ENEMIES //
        private Point rectangleFunc(Point C, float rad)
        {
            //(x – h)^2 + (y – k)^2 = r ^ 2, where(h, k) represents the coordinates of the center of the circle, and r represents the radius of the circle.
            // create random angle val: between 0 and 360 [0,360)
            // 
            Random x = new Random();
            int angle = (x.Next(0,360));

            double ylen = rad * Math.Sin(angle);
            double xlen = rad * Math.Cos(angle);

            Point spawnPoint = new Point(C.X + (int)xlen, C.Y + (int)ylen);
            return spawnPoint;
        }
        private void enemySpawner(string enemyType)
        {
            // might need an enemy class with multiple picture box objects to hide that pictureboxes are not pngs and have whitespace.
            // and also for animations.

            PictureBox enemy = new PictureBox();
            enemy.Location = rectangleFunc(center, radius);     // spawn location
            enemy.Size = new Size(32, 32);
            enemy.ImageLocation = "images\\enemytest3.png";     // imagelocation
            this.Controls.Add(enemy);                           // controls.add
            enemy.BringToFront();                               // bringtofront

            // visible (if needed)
            //enemy.
            // subscribe to AI event.

            // maybe create a circle (or a function that consists these 4 points and is a circle graph?)
            // i like the circle function idea.
            // circle should be a little bigger than actual map borders. (guessing 16 pixels for now.)
        }
        private void basicEnemyMovement(object sender, EventArgs e, int speed)
        {

        }
        private void spawnTestTimer_Tick(object sender, EventArgs e)    // 100ms.
        {
            // according to the wave, have a list of enemies to spawn.
            // every tick, spawn an enemy, on a random location on the circle function.
            // call enemyspawner in the amount of wave enemies times, with enemy input
            enemySpawner("Test");
        }
        private void skipWave(object sender, EventArgs e)               // button event.
        {

        }


    }
    // TILE // 
    public class Tile
    {
        // if tile state is above a certain point, should not check for add unit. unit states will fill states until that point.
        public PictureBox tileSprite; // rectangle should become a picturebox? some form to hold tile picture
        public Point location; // Location of tile
        public state State; 
        public int[,] id;
        public Tile(Point locat)
        {
            location = locat;

            tileSprite = new PictureBox();
            tileSprite.Size = new Size(32, 32);
            tileSprite.Location = location;

            SetState(state.Empty);
        }

        public void SetState(state stateinf)
        {
            State = stateinf;
            switch (stateinf)
            {
                case state.Target: 
                    tileSprite.ImageLocation = "images\\tile-target.png";
                    break;
                case state.Corrupted:
                    tileSprite.ImageLocation = "images\\tile-corrupted.png";
                    break;
                case state.Resource:
                    tileSprite.ImageLocation = "images\\tile-resource.png";
                    break;
                case state.Boulder:
                    tileSprite.ImageLocation = "images\\tile-boulder.png";
                    break;
                case state.Empty:
                    tileSprite.ImageLocation = "images\\tile-empty.png";
                    break;
                case state.Platformed:
                    tileSprite.ImageLocation = "images\\tile-platform.png";
                    // platformed sprite
                    break;
                case state.t1:
                    tileSprite.ImageLocation = "images\\tile-u1.png";
                    // platform + unit1 sprite
                    break;
                case state.t2:
                    tileSprite.ImageLocation = "images\\tile-u1.png";  // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    // platform + unit2 sprite
                    break;
                case state.Mine:
                    tileSprite.ImageLocation = "images\\tile-platform.png";  // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    // Mine sprite
                    break;
            }
        }
    }
    // Tile ENUM //
    public enum state
    {
        Target,             // 0
        Corrupted,          // 1
        Resource,           // 2
        Boulder,            // 3
        Empty,              // 4
        Platformed,         // 5
        t1,                 // 6
        t2,                 // 7
        Mine,               // 8    
    }
}
