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
        Tile[,] tileMap;                                // grid map
        List<Tile> targetables = new List<Tile>();     // tiles that are targetable by enemies. !!!!!!!!!!!!!!!!!!!!!!!!!!! RESOURCES SHOULD BE ADDED WHEM THEY BECOME A MINE.
        Button[] activeMenus;                           // List of active buttons / menus to regulate clicks.
        bool Mstate;                                    // Menu State.
        // spawner
        static int offset = 40;
        Point topleft;
        Point topright;
        Point bottomleft;
        Point bottomright;
        Point center;
        float radius;
        int spawntimer = 10;
        // random
        Random roll;
        // All Enemies and Units
        List<Enemy> enemylist = new List<Enemy>();
        List<Tower> unitlist = new List<Tower>();
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
            int bot = 32 * 30;    // size of tile * amount of tile
            int right = 32 * 30;  // size of tile * amount of tile
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

                    tileMap[xt, yt] = currentTile;
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
            Point obj = new Point(roll.Next(12, 18), roll.Next(12, 18));
            targetables.Add(tileMap[obj.X, obj.Y]); 
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
                tileImplement(genArray[rep + 1], state.Corrupted);
            }


            // resources
            for (int amount = 0; amount < 3; amount++)
            {
                Point resourcePoint;
                do
                {
                    resourcePoint = new Point(roll.Next(0, 30), roll.Next(0, 30));
                } while (tileGenerated(resourcePoint, genArray, last));
                targetables.Add(tileMap[resourcePoint.X, resourcePoint.Y]);
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
                int levelreset = (3 - times % 3) * ((x + 3) / (((times + 3) / 3) * 3)); // if times > 3, set it to 3 until x > 3 for every 3 loop. 6 3 3 , 5 3 2, 4 3 1, 7 3 3 1, 8 3 3 2, 2 = 2
                int xfix = (((3 - (levelreset) - 1) * 16) - (32 * (x - ((x / 3) * 3))));
                int yfix = ((x / 3) * 20);


                Button u1 = new Button();
                u1.Text = "Creation" + (x + 1);
                u1.Location = new Point(tl.location.X - xfix, (tl.location.Y - 20) - yfix);
                u1.Size = new Size(32, 20);
                u1.Name = "active" + (x + 1);
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
                    for (int unit = 0; 2 > unit; unit++)
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
            for (int ind = 0; activeMenus.Length > ind; ind++)
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

        // Spawn System / Waves //
        private Point rectangleFunc(Point C, float rad)
        {
            //(x – h)^2 + (y – k)^2 = r ^ 2, where(h, k) represents the coordinates of the center of the circle, and r represents the radius of the circle.
            // create random angle val: between 0 and 360 [0,360)
            // 
            Random x = new Random();
            int angle = (x.Next(0, 360));

            double ylen = rad * Math.Sin(angle);
            double xlen = rad * Math.Cos(angle);

            Point spawnPoint = new Point(C.X + (int)xlen, C.Y + (int)ylen);
            return spawnPoint;
        }
        private void enemySpawner(string enemyType, Point spawnpoint)
        {
            // might need an enemy class with multiple picture box objects to hide that pictureboxes are not pngs and have whitespace.
            // and also for animations.
            PictureBox img = new PictureBox();                              // image declaration
            img.Location = spawnpoint;                                      // location 
            img.Size = new Size(32, 32);                                    // imagesize
            img.ImageLocation = "images\\enemytest3.png";                   // imagelocation
            ChaserEnemy currentEnemy = new ChaserEnemy(targetables, img);   // enemy declaration
            this.Controls.Add(currentEnemy.enemySprite);                    // controls.add
            currentEnemy.enemySprite.BringToFront();                        // bringtofront
            enemylist.Add(currentEnemy);

            // visible (if needed)
            //enemy.
            // subscribe to AI event.

            // maybe create a circle (or a function that consists these 4 points and is a circle graph?)
            // i like the circle function idea.
            // circle should be a little bigger than actual map borders. (guessing 16 pixels for now.)
        }
        private void spawnTestTimer_Tick(object sender, EventArgs e)    // 100ms.
        {
            //// according to the wave, have a list of enemies to spawn.
            //// every tick, spawn an enemy, on a random location on the circle function.
            //// call enemyspawner in the amount of wave enemies times, with enemy input
            //spawntimer -= 1;
            //if (spawntimer < 1)
            //{
            //    Point spawn = rectangleFunc(center,radius);  
            //    enemySpawner("Test",spawn);
            //    spawntimer = 10;
            //}
            try
            {
                foreach (Enemy chaser in enemylist)
                {
                    chaser.pathfind();
                    chaser.move();
                }
            }
            catch (NullReferenceException)
            {
                
            }
        }
        private void skipWave(object sender, EventArgs e)               // button event.
        {

        }

        private void enemyPathfind(object sender, EventArgs e, int speed, PictureBox enemy)
        {
            
        }

        private void ButtonSpawner(object sender, EventArgs e)
        {
            Point current = new Point(tileMap[29, 29].location.X + 150, tileMap[29, 29].location.Y);
            enemySpawner("type", current);
        }


        // CUSTOM OPTIONS
        private void PauseGame_Click(object sender, EventArgs e) // doesnt work.
        {
            Control[] list = this.Controls.Find("spawnTestTimer", true);
            if(list[0].Enabled == false) list[0].Enabled = true;
            else list[0].Enabled = false;

            // for this to work properly, need to create timers in code and add them to a list to reach quickly.
            // find all timers in the game, stop it
            // if stopped, continue the game.
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
    // ENEMIES
    abstract class Enemy // make enemy an abstract class to create subclasses of enemies from it?
    {
        public PictureBox enemySprite;
        public Random roll;
        public float speed;
        public Tile target;
        public bool inMap; // if inMap, A*. else move towards target.
        //public Enemy()
        //{
        //    // every class of enemy will have its own predetermined stats
        //    // Waves might change enemy stats after some wave.
        //}
        public abstract void pathfind(Point topleft, Point bottomright);
        public abstract void move();
        public abstract void targeting(List<Tile> targetables);
        public abstract bool mapLocationCheck(Point topleft, Point bottomright);
        public void pathfind_new(Point start, Point target)
        {
            List<Node> searchList = new List<Node>();
            List<Node> processedList = new List<Node>();
            // Start Node
            Node currentNode = new Node(start, target, );
            searchList.Add(currentNode);
            bool targetFound = false;

            while (!targetFound)
            {
                List<Node> currentList = currentNode.adjacentCheck();                              //!!!!!!!!!!!!!!!!!!! NULL FUNCTION
                // collects available adjacent tiles.
                searchList.AddRange(currentList);
                searchList.Remove(currentNode);
                processedList.Add(currentNode);
                currentNode = FindNewNode(searchList);
                // finds GHT value, chooses that node and returns it.
                targetFound = TargetReachCheck(currentNode);
                // checks if currentNode is target, sets it to targetFound.
            }
        }
    }
    class ChaserEnemy : Enemy
    {
        Point direction; // current direction, aside from path.
        public ChaserEnemy(List<Tile>targetables, PictureBox img)
        {
            speed = 15f;
            enemySprite = img;
            roll = new Random();
            inMap = false;
            targeting(targetables);
        }
        public override void pathfind(Point topleft, Point bottomright)
        {
            inMap = mapLocationCheck(topleft, bottomright);
            if (inMap)
            {
                // A*
            }
            else
            {
                direction = new Point(target.location.X - enemySprite.Location.X, target.location.Y - enemySprite.Location.Y);

            }
        }
        public override void move()
        {
            float x_sqrt = direction.X * direction.X;
            float y_sqrt = direction.Y * direction.Y;
            double distance = Math.Sqrt(x_sqrt + y_sqrt);
            enemySprite.Location = new Point(  (int)(enemySprite.Location.X + ((float)direction.X / distance) * speed) , (int)(enemySprite.Location.Y + ((float)direction.Y / distance) * speed));
        }
        public override void targeting(List<Tile>targetables)
        {
            int targetId = roll.Next(0, targetables.Count);
            target = targetables[targetId];
        }
        public override bool mapLocationCheck(Point topleft, Point bottomright)
        {
            bool tlCheck = (this.enemySprite.Location.X > topleft.X) & (this.enemySprite.Location.Y < topleft.Y);          // topleft check
            bool brCheck = (this.enemySprite.Location.X < topleft.X) & (this.enemySprite.Location.Y > bottomright.Y);                                                                    // botright check
            if (tlCheck & brCheck)
            {
                return true;
            }
            return false;
        }
        //Vector2 SpeedCalc(Vector2 direction)
        //{
        //    float distance = PlayerDistance();
        //    Vector2 netspeed = new Vector2(direction.x / distance, direction.y / distance);
        //    return netspeed;
        //} // setting speed to one certain value using sin and cos.
        //float PlayerDistance() // finding distance to player
        //{
        //    Vector2 DistanceVector = DirectionCalc();
        //    float vectorx_psg = (DistanceVector.x * DistanceVector.x);
        //    float vectory_psg = (DistanceVector.y * DistanceVector.y);
        //    float distance = Mathf.Sqrt(vectorx_psg + vectory_psg);
        //    return distance;
        //}
    }
    // TOWERS
    abstract class Tower // make Tower an abstract class to create subclasses of tower units from it?
    {
        int attackSpeed;
        public abstract void target();
    }
    // A* PATHFINDING NODES
    public class Node
    {
        int G, H, T; // G is travelled distance from start. // H is direct distance to target. Second checked value // T is F + H, first checked Value
        bool Start, Target, blocked;
        Point loc;

        int limit = 29;
        //public Node StartNode(Point target, Point location)
        //{
        //    // DistanceCalculations
        //    G = 0;
        //    H = calculateH();
        //    T = G + H;
        //    // Booleans
        //    Start = true;
        //    blocked = false;
        //    Target = false;
        //    // Location
        //    loc = start;
        //    return;
        //}
        public Node (Point location)  // !!!!!!!!!!!!!!!!!!!!!!!! SKIPPED CONSTRUCTOR, could not understand what I wanted to do here.
        {
            // DistanceCalculations
            G = calcG();
            H = calcH();
            T = G + H;
            // Booleans
            Start = false;
            blocked = false;
            Target = false;

            SetTileState(this);
        }
        public void SetTileState(Node current)
        {
            // Start, Target, Location check.
        }
        public List<Node> adjacentCheck()
        {
            List<Node> searchable = new List<Node>();
            bool[] existing = CheckExisting();  // 4 length array.
            // Finds existing tiles and returns them in order. Clockwise start from top. 0 = top, 1 = right, 2 = bottom, 3 = left.
            // if tile exists return true if not return false.
            for(int i = 0; i<existing.Length; i++)
            {
                if (existing[i])
                {
                    Point newTileLoc = calculateLocation(i); // from i information can be calculated with simple switch. keys are given above.
                    Node current = new Node(start, target, newTileLoc);
                    calcG(i, calculateH(newTileLoc));
                    if (!current.blocked)
                    {
                        searchable.Add(current);
                    }
                }
            }
            return searchable;
        }
        public void calcG() // Im not sure where the return will be?
        {

        }
        public int calcH(Point location)
        {
            
        }
        public void newLocCalc(int i)
        {
            switch (i)
            {
                case 0:
                    // top
                    break;
                case 1:
                    // right
                    break;
                case 2:
                    // bottom
                    break;
                case 3:
                    // left
                    break;
            }
        }

    }
}
