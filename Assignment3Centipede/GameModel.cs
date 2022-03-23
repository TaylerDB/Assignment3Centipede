using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3Centipede
{
    class GameModel
    {
        private GraphicsDeviceManager m_graphics;


        // Consts
        const double fireRate = 0.15f;
        const double mushroomSpawnRate = 0.5f;
        private const int shipMovementSpeed = 5;
        const int bulletSpeed = 10;


        private SpriteFont m_font;
        private SpriteFont m_bigFont;
        private const string Win_MESSAGE = "YOU WIN!";
        private const string LOST_MESSAGE = "YOU LOST!";

        int score = 0;
        int highScore;

        public int getHighScore
        {
            get { return highScore; }
        }
        
        // Rectangles
        private Rectangle m_ShipBox;
        private Rectangle m_ShipBoxLife1;
        private Rectangle m_ShipBoxLife2;
        private Rectangle m_ShipBoxLife3;

        private Rectangle m_FleeBox;
        private Rectangle m_SpiderBox;
        private Rectangle m_ScorpionBox;

        //private Rectangle m_CentipedeBox;

        // Textures
        private Texture2D m_NormMush1Texture;
        private Texture2D m_NormMush2Texture;
        private Texture2D m_NormMush3Texture;
        private Texture2D m_NormMush4Texture;

        private Texture2D m_BulletTexture;
        private Texture2D m_ShipTexture;

        private Texture2D m_CentipedeTexture;


        HelpView helpView = new HelpView();
        Ship ship = new Ship();
        bool shipAlive = true;

        private Objects.Flea flea;
        private Objects.Spider spider;
        private Objects.Scorpion scorpion;
        //private Objects.Centipede centipede;

        private AnimatedSprite fleaRenderer;
        private AnimatedSprite centipedeRenderer;
        private AnimatedSprite spiderRenderer;
        private AnimatedSprite scorpionRenderer;

        bool gameWon = false;
        bool gameLost = false;

        bool spawnFlee = true;
        int fleeMushCount = 0;

        double centYPos = 0;
        int randXSpider = 0;

        bool goUp = false;
        bool goDown = true;
        bool centStart = true;
        bool hitLeftMush = false;
        bool hitRightMush = false;
        bool moveCentUp = false;
        bool moveCentDown = false;
        bool moveCentLeft = true;
        bool moveCentRight = false;

        bool moveSpiderUp = false;
        bool moveSpiderDown = false;

        TimeSpan fireRateTimer = TimeSpan.FromSeconds(0);
        TimeSpan mushroomSpawnTimer = TimeSpan.FromSeconds(0);

        // Data structures
        List<Rectangle> mushroomList = new List<Rectangle>();
        //List<object> 

        // Default constructor
        public GameModel() { }

        public GameModel(GraphicsDeviceManager m_graphics)
        {
            this.m_graphics = m_graphics;
        }

        public List<Rectangle> MushroomList
        {
            get { return mushroomList; }
            set { mushroomList = value; }
        }

        List<object> mushroomClassList = new List<object>();
        List<object> centipedeList = new List<object>();
        List<Rectangle> bulletList = new List<Rectangle>();

        public void loadContent(ContentManager contentManager)
        {
            // Setup font
            m_font = contentManager.Load<SpriteFont>("Fonts/game");
            m_bigFont = contentManager.Load<SpriteFont>("Fonts/menu");

            // Setup textures
            m_NormMush1Texture = contentManager.Load<Texture2D>("Images/MushNorm/NormMush0");
            m_NormMush2Texture = contentManager.Load<Texture2D>("Images/MushNorm/NormMush1");
            m_NormMush3Texture = contentManager.Load<Texture2D>("Images/MushNorm/NormMush2");
            m_NormMush4Texture = contentManager.Load<Texture2D>("Images/MushNorm/NormMush3");
            m_BulletTexture = contentManager.Load<Texture2D>("Images/Ship/Bullet");
            m_ShipTexture = contentManager.Load<Texture2D>("Images/Ship/Ship");

            m_CentipedeTexture = contentManager.Load<Texture2D>("Images/Centipede/centipede_part");

            // Setup random for mushroom
            Random xPos = new Random();
            Random yPos = new Random();
            int randX = 0;
            int randY = 0;

            // Populate Mushroom list and class
            for (int i = 0; i < 75; i++)
            {
                // Add to mushroom class
                mushroomClassList.Add(new Mushroom());

                // Make mushrooms in a grid size
                randX = xPos.Next(m_graphics.GraphicsDevice.Viewport.Width - 25);
                while (randX % 30 != 0)
                {
                    randX = xPos.Next(m_graphics.GraphicsDevice.Viewport.Width - 25);
                }

                randY = yPos.Next(50, m_graphics.GraphicsDevice.Viewport.Height - 100);
                while (randY % 30 != 0)
                {
                    randY = yPos.Next(50, m_graphics.GraphicsDevice.Viewport.Height - 100);
                }

                // Add to mushroom list
                mushroomList.Add(new Rectangle(randX, randY, 25, 25));

                //mushroomList.Add(new Rectangle(200, 60, 25, 25));

            }

            // Set up player
            m_ShipBox = new Rectangle(m_graphics.GraphicsDevice.Viewport.Width / 2, m_graphics.GraphicsDevice.Viewport.Height - 25, 25, 25);
            ship = new Ship(m_ShipBox, m_graphics);

            // Draw lives
            m_ShipBoxLife1 = new Rectangle(m_graphics.GraphicsDevice.Viewport.Width / 5, 10, 25, 25);
            m_ShipBoxLife2 = new Rectangle((m_graphics.GraphicsDevice.Viewport.Width / 5) + 30, 10, 25, 25);
            m_ShipBoxLife3 = new Rectangle((m_graphics.GraphicsDevice.Viewport.Width / 5) + 60, 10, 25, 25);

            // Setup flea animation
            fleaRenderer = new AnimatedSprite(
                contentManager.Load<Texture2D>("Images/spritesheet-flee"),
                new int[] { 100, 100, 100, 100 }
                );

            // Setup centipede animation
            centipedeRenderer = new AnimatedSprite(
                contentManager.Load<Texture2D>("Images/Centipede/centipede_side"),
                new int[] { 100, 100, 100, 100, 100, 100, 100, 100 }
                );

            // Setup spider animation
            spiderRenderer = new AnimatedSprite(
                contentManager.Load<Texture2D>("Images/Spider"),
                new int[] { 200, 200, 200, 200, 200, 200, 200, 200 }
                );

            // Setup scorpion animation
            scorpionRenderer = new AnimatedSprite(
                contentManager.Load<Texture2D>("Images/Scorpion"),
                new int[] { 200, 200, 200, 200 }
                );

            // Set up cenitpede
            //m_CentipedeBox = new Rectangle(m_graphics.GraphicsDevice.Viewport.Width / 6, 50, 25, 25);

            for (int i = 0; i < 3; i++)
            {
                Objects.Centipede centipede;
                centipede = new Objects.Centipede(
                    new Vector2(25, 25), // image size
                    new Vector2((m_graphics.GraphicsDevice.Viewport.Width / 2) - (i * 30), 50), // starting x y pos
                    3 / 1000.0, // Pixels per second
                    new Rectangle(),
                    m_graphics);

                centipedeList.Add(centipede);
            }

        }

        public void initialize() { }

        public void processInput(GameTime gameTime)
        {
            // Check if ship is alive or game is still running
            if (shipAlive == true && !gameWon && !gameLost)
            {
                // Check for ship movement
                moveShip();

                // Shoot bullets
                if (Keyboard.GetState().IsKeyDown(helpView.Shoot))
                {
                    // Check game time for fire rate
                    if (fireRateTimer.TotalSeconds > fireRate)
                    {
                        // Add bullet to list
                        bulletList.Add(new Rectangle(m_ShipBox.X + 5, m_ShipBox.Y - 25, 15, 30));

                        // Reset fire rate timer to zero
                        fireRateTimer = TimeSpan.FromSeconds(0);
                    }
                }
            }
        }

        private void moveShip()
        {
            // Move right
            if ((Keyboard.GetState().IsKeyDown(helpView.MoveRight)) && (m_ShipBox.X < m_graphics.GraphicsDevice.Viewport.Width - 30))
            {
                //int a = gameModel.MushroomList.Count;
                int canMoveRight = 0;

                for (int i = 0; i < mushroomList.Count; i++)
                {
                    var mushroom = mushroomList[i];
                    //                                                                                                                          - 30
                    if ((m_ShipBox.X < mushroom.X - 30) || (m_ShipBox.X >= mushroom.X + 30) || (m_ShipBox.Y >= mushroom.Y + 25) || (m_ShipBox.Y + 30 <= mushroom.Y))
                    {
                        // Increment if it can move
                        canMoveRight++;                        
                    }
                    mushroomList[i] = mushroom;
                }

                // Only move if all mushrooms are out of the way
                if (canMoveRight == mushroomList.Count)
                {
                    //m_ShipBox.Y -= shipMovementSpeed;
                    ship.moveRight();
                    m_ShipBox.X = ship.xPos;
                }
            }

            // Move left
            if ((Keyboard.GetState().IsKeyDown(helpView.MoveLeft)) && (m_ShipBox.X >= 5))
            {
                int canMoveLeft = 0;

                for (int i = 0; i < mushroomList.Count; i++)
                {
                    var mushroom = mushroomList[i];
                    //                                                                                              + 30                            -30
                    if ((m_ShipBox.X > mushroom.X + 30) || (m_ShipBox.X <= mushroom.X - 30) || (m_ShipBox.Y >= mushroom.Y + 25) || (m_ShipBox.Y + 30 <= mushroom.Y))
                    {
                        // Increment if it can move
                        canMoveLeft++;
                    }
                    mushroomList[i] = mushroom;
                }

                // Only move if all mushrooms are out of the way
                if (canMoveLeft == mushroomList.Count)
                {
                    //m_ShipBox.Y -= shipMovementSpeed;
                    ship.moveLeft();
                    m_ShipBox.X = ship.xPos;
                }
            }
            
            // Move up
            if ((Keyboard.GetState().IsKeyDown(helpView.MoveUp)) && (m_ShipBox.Y >= (m_graphics.GraphicsDevice.Viewport.Height * .7)))
            {
                int canMoveUp = 0;
                for (int i = 0; i < mushroomList.Count; i++)
                {
                    var mushroom = mushroomList[i];
                    //   Left  +30                                     Right - 30                               + 30                                    - 30
                    if ((m_ShipBox.X >= mushroom.X + 30) || (m_ShipBox.X <= mushroom.X - 30) || (m_ShipBox.Y >= mushroom.Y + 30) || (m_ShipBox.Y < mushroom.Y))
                    {
                        // Increment if it can move
                        canMoveUp++;

                    }
                    mushroomList[i] = mushroom;
                }

                // Only move if all mushrooms are out of the way
                if (canMoveUp == mushroomList.Count)
                {
                    //m_ShipBox.Y -= shipMovementSpeed;
                    ship.moveUp();
                    m_ShipBox.Y = ship.yPos;
                }
            }

            // Move down
            if ((Keyboard.GetState().IsKeyDown(helpView.MoveDown)) && (m_ShipBox.Y != m_graphics.GraphicsDevice.Viewport.Height - 25))
            {
                int canMoveDown = 0;

                for (int i = 0; i < mushroomList.Count; i++)
                {
                    var mushroom = mushroomList[i];
                    //                         + 30                               -30                                 + 30                         - 30
                    if ((m_ShipBox.X >= mushroom.X + 30) || (m_ShipBox.X <= mushroom.X -30) || (m_ShipBox.Y >= mushroom.Y) || (m_ShipBox.Y < mushroom.Y - 30))
                    {
                        // Increment if it can move
                        canMoveDown++;
                    }
                    mushroomList[i] = mushroom;
                }

                // Only move if all mushrooms are out of the way
                if (canMoveDown == mushroomList.Count)
                {
                    //m_ShipBox.Y -= shipMovementSpeed;
                    ship.moveDown();
                    m_ShipBox.Y = ship.yPos;
                }
            }
        }
   

        public void update(GameTime gameTime)
        {
            //m_player.update(gameTime);
            //m_centipede.update(gameTime);
            updateBullets(gameTime);

            // Take care of flee
            updateFlea(gameTime);

            // Take care of centipede
            updateCentipede(gameTime);

            // Spawn Spider if score is above 100
            if (score > 100 && spider == null)
            {
                // Get random x flee spawn position
                Random xPos = new Random();
                randXSpider = xPos.Next(0,2);

                // Spawn on right
                if (randXSpider == 0)
                {
                    randXSpider = m_graphics.GraphicsDevice.Viewport.Width - 25;
                }
                // Spawn on left
                else
                    randXSpider = 40;

                // Spawn spider
                if (spider == null)
                {
                    //spawnFlee = false;
                    spider = new Objects.Spider(
                        new Vector2(40, 40), // image size
                        new Vector2(randXSpider,  800), // starting x y pos
                        200 / 1000.0, // Pixels per second
                        m_SpiderBox,
                        m_graphics);
                }
            }

            // Update spider
            if (spider != null)
            {
                // Move spider right
                if ((spider.xPos < m_graphics.GraphicsDevice.Viewport.Width - 40) && randXSpider == 40)
                {
                    if (spider.yPos > m_graphics.GraphicsDevice.Viewport.Height * .7 && !moveSpiderDown)
                    {
                        moveSpiderDown = false;
                        moveSpiderUp = true;
                    }
                    if (moveSpiderUp)
                    { 
                        spider.moveUp(gameTime);
                        spider.moveRight(gameTime);
                    }

                    if (spider.yPos <= m_graphics.GraphicsDevice.Viewport.Height * .7)
                    {
                        moveSpiderUp = false;
                        moveSpiderDown = true;
                    }

                    if (moveSpiderDown)
                    { 
                        spider.moveDown(gameTime);
                        spider.moveRight(gameTime);
                    }

                    if (spider.yPos >= m_graphics.GraphicsDevice.Viewport.Height - 40)
                    {
                        moveSpiderUp = true;
                        moveSpiderDown = false;
                    }
                }
                if (spider.xPos > (m_graphics.GraphicsDevice.Viewport.Width - 40))
                {
                    randXSpider = m_graphics.GraphicsDevice.Viewport.Width - 25;
                }

                // Move spider left
                if ((spider.xPos > 40) && randXSpider == m_graphics.GraphicsDevice.Viewport.Width - 25)
                {
                    if (spider.yPos > m_graphics.GraphicsDevice.Viewport.Height * .7 && !moveSpiderDown)
                    {
                        moveSpiderDown = false;
                        moveSpiderUp = true;
                    }
                    if (moveSpiderUp)
                    {
                        spider.moveUp(gameTime);
                        spider.moveLeft(gameTime);
                    }

                    if (spider.yPos <= m_graphics.GraphicsDevice.Viewport.Height * .7)
                    {
                        moveSpiderUp = false;
                        moveSpiderDown = true;
                    }

                    if (moveSpiderDown)
                    {
                        spider.moveDown(gameTime);
                        spider.moveLeft(gameTime);
                    }

                    if (spider.yPos >= m_graphics.GraphicsDevice.Viewport.Height - 40)
                    {
                        moveSpiderUp = true;
                        moveSpiderDown = false;
                    }
                }
                if (spider.xPos < 40)
                {
                    randXSpider = 40;
                }

                // Check collison on ship
                if ((m_ShipBox.X >= spider.xPos - 30) && (m_ShipBox.X <= spider.xPos + 30) && (m_ShipBox.Y <= spider.yPos + 20) && shipAlive)
                {
                    // Kill ship
                    killShip();
                }
            }


            // Spawn scorpion if score is greater than 300
            if (score > 100 && scorpion == null)
            {
                // Get random x flee spawn position
                Random yPos = new Random();
                int randY = yPos.Next(50, m_graphics.GraphicsDevice.Viewport.Height - 100);
                while (randY % 30 != 0)
                {
                    randY = yPos.Next(50, m_graphics.GraphicsDevice.Viewport.Height - 100);
                }

                // Spawn flee
                if (scorpion == null)
                {
                    //spawnFlee = false;
                    scorpion = new Objects.Scorpion(
                        new Vector2(30, 30), // image size
                        new Vector2(25, randY), // starting x y pos
                        200 / 1000.0, // Pixels per second
                        m_ScorpionBox,
                        m_graphics);
                }
            }

            if (scorpion != null)
            {
                if (scorpion.xPos < m_graphics.GraphicsDevice.Viewport.Width)
                {
                    scorpion.moveRight(gameTime);
                }

                if (scorpion.xPos > m_graphics.GraphicsDevice.Viewport.Width)
                {
                    scorpion = null;
                }
            }
            
            // Update flee animation
            fleaRenderer.update(gameTime);

            // Update centipede animation
            centipedeRenderer.update(gameTime);

            // Update spider animation
            spiderRenderer.update(gameTime);

            // Update scorpion animation
            scorpionRenderer.update(gameTime);
        }

        private void updateBullets(GameTime gameTime)
        {
            // Increase fire rate timer
            fireRateTimer += gameTime.ElapsedGameTime;

            // Update bullet position
            if (bulletList.Count > 0)
            {
                for (int i = 0; i < bulletList.Count; i++)
                {
                    bool bulletAlive = true;

                    var bullet = bulletList[i];
                    bullet.Y -= bulletSpeed;

                    // Check for collison on mushrooms
                    for (int m = 0; m < mushroomList.Count; m++)
                    {
                        // Get a copy
                        var mushList = mushroomList[m];
                        var mush = (Mushroom)mushroomClassList.ElementAt(m);

                        // Check if bullet is within parameters of mushroom
                        if ((bullet.X >= mushList.X - 25) && (bullet.X <= mushList.X + 25) && (bullet.Y >= mushList.Y - 25) && (bullet.Y <= mushList.Y + 25))
                        {
                            // Tell mushroom
                            mush.hitMushroom();

                            // Update score
                            score += 4;

                            // Remove bullet
                            if (i < bulletList.Count)
                            {
                                bulletList.RemoveAt(i);
                                bulletAlive = false;
                            }
                        }

                        // Check if mushroom needs to be removed
                        if (mush.Hit > 3)
                        {
                            mushroomClassList.RemoveAt(m);
                            mushroomList.RemoveAt(m);
                        }
                        else
                        {
                            // Set copy back to original
                            mushroomClassList[m] = mush;
                            mushroomList[m] = mushList;
                        }
                    }
                  
                    // If flee is alive
                    if (flea != null)
                    {
                        // Check for bullet collison on flee
                        if ((bullet.X >= flea.xPos - 25) && (bullet.X <= flea.xPos + 25) && (bullet.Y <= flea.yPos + 20))
                        {
                            // Hit flee
                            flea.hit();

                            // Check if flee has gotten hit twice
                            if (flea.HitFlee >= 2)
                            {
                                // Kill flee
                                flea = null;
                                fleeMushCount = 0;

                                // Update score
                                score += 200;
                            }

                            // Remove bullet
                            if (i < bulletList.Count)
                            {
                                bulletList.RemoveAt(i);
                                bulletAlive = false;
                            }
                        }
                    }

                    //// Check collison on centipede
                    for (int c = 0; c < centipedeList.Count; c++)
                    {
                        var cent = (Objects.Centipede)centipedeList.ElementAt(c);

                        if (cent != null)
                        {
                            if ((bullet.X > cent.xPos - 25) && (bullet.X < cent.xPos + 25) && (bullet.Y <= cent.yPos + 20) && (bullet.Y > cent.yPos - 20))
                            {
                                if (centipedeList.Count > 1)
                                {
                                    // Remove centipede piece
                                    centipedeList.RemoveAt(c);

                                    mushroomList.Add(new Rectangle((int)cent.xPos, (int)cent.yPos, 25, 25));
                                    mushroomClassList.Add(new Mushroom());

                                    // Spawn a new mushroom


                                    // Update score
                                    score += 300;
                                }

                                // You win
                                else
                                {
                                    // Stop drawing centipede
                                    cent = null;
                                    centipedeList[c] = cent;
                                    // Update high score
                                    highScore = score;
                                    gameWon = true;
                                }

                                // Remove bullet
                                if (i < bulletList.Count)
                                {
                                    bulletList.RemoveAt(i);
                                    bulletAlive = false;
                                }
                            }
                        }
                        else
                        {
                            centipedeList[c] = cent;
                        }
                    }

                    // If spider is alive
                    if (spider != null)
                    {
                        // Check for bullet collison on spider
                        if ((bullet.X >= spider.xPos - 25) && (bullet.X <= spider.xPos + 25) && (bullet.Y <= spider.yPos + 20))
                        {
                            // Kill spider
                            spider = null;
                            
                            // Update score
                            score += 400;

                            // Remove bullet
                            if (i < bulletList.Count)
                            {
                                bulletList.RemoveAt(i);
                                bulletAlive = false;
                            }
                        }
                    }

                    // If scorpion is alive
                    if (scorpion != null)
                    {
                        // Check for bullet collison on scorpion
                        if ((bullet.X >= scorpion.xPos - 25) && (bullet.X <= scorpion.xPos + 25) && (bullet.Y <= scorpion.yPos + 20))
                        {
                            // Kill spider
                            scorpion = null;

                            // Update score
                            score += 1000;

                            // Remove bullet
                            if (i < bulletList.Count)
                            {
                                bulletList.RemoveAt(i);
                                bulletAlive = false;
                            }
                        }
                    }

                    // Check if bullet is still alive
                    if (bulletAlive)
                    {
                        // Kill bullet if it reaches top of the screen
                        if (bullet.Y < 0)
                            bulletList.RemoveAt(i);

                        // Set copy back to original
                        else
                            bulletList[i] = bullet;
                    }
                }
            }
        }

        private void updateFlea(GameTime gameTime)
        {
            // Spawn flee for every 5 mushroom deaths
            if (mushroomList.Count < 70 && flea == null)
            {
                // Get random x flee spawn position
                Random xPos = new Random();
                int randX = xPos.Next(30, m_graphics.GraphicsDevice.Viewport.Width - 25);

                while (randX % 30 != 0)
                {
                    randX = xPos.Next(50, m_graphics.GraphicsDevice.Viewport.Width - 25);
                }

                // Spawn flee
                if (flea == null)
                {
                    spawnFlee = false;
                    flea = new Objects.Flea(
                        new Vector2(25, 25), // image size
                        new Vector2(randX, 0), // starting x y pos
                        200 / 1000.0, // Pixels per second
                        m_FleeBox,
                        m_graphics);
                }
            }

            // Flee is alive
            if (flea != null)
            {
                // Move flee down screen
                flea.moveDown(gameTime);

                // Update time
                mushroomSpawnTimer += gameTime.ElapsedGameTime;

                // Only spawn 5 mushrooms per flee
                if (fleeMushCount < 6 && flea.yPos > 50)
                {
                    // Check if enough time has passed to spawn mushroom
                    if (mushroomSpawnTimer.TotalSeconds > mushroomSpawnRate)
                    {
                        // Increment counter
                        fleeMushCount++;

                        // Add new mushroom
                        mushroomClassList.Add(new Mushroom());
                        mushroomList.Add(new Rectangle((int)flea.xPos, (int)flea.yPos, 25, 25));

                        // Reset timer
                        mushroomSpawnTimer = TimeSpan.FromSeconds(0);
                    }
                }

                // Check collison on ship
                if ((m_ShipBox.X >= flea.xPos - 30) && (m_ShipBox.X <= flea.xPos + 30) && (m_ShipBox.Y <= flea.yPos + 20) && shipAlive)
                {
                    // Kill ship
                    killShip();
                }

            }

            // Kill flee if off screen
            if (flea != null)
            {
                if (flea.yPos > m_graphics.GraphicsDevice.Viewport.Height)
                {
                    fleeMushCount = 0;
                    spawnFlee = true;
                    flea = null;
                }
            }
        }

        private void updateCentipede(GameTime gameTime)
        {
            // If at wall or mushroom go up or down
            for (int i = 0; i < centipedeList.Count; i++)
            {
                //var mushList = mushroomList[m];
                var cent = (Objects.Centipede)centipedeList.ElementAt(i);

                for (int m = 0; m < mushroomList.Count; m++)
                {
                    // Get a copy
                    var mushList = mushroomList[m];

                    if (cent != null)
                    {
                        if (centStart)
                        {
                            centStart = false;
                            //moveCentDown = true;
                            moveCentLeft = true;
                        }

                        // If at top go down
                        if (cent.yPos <= 30)
                        {
                            goUp = false;
                            goDown = true;
                        }

                        // If at bottom go up
                        if (cent.yPos >= m_graphics.GraphicsDevice.Viewport.Height - 30)
                        {
                            goDown = false;
                            goUp = true;
                        }

                        // Move left
                        if (moveCentLeft)
                        {
                            // Move centipede left
                            cent.moveLeft(gameTime);
                            centYPos = cent.yPos;
                        }

                        // Stop on left side and go down
                        if (cent.xPos <= 15 && goDown)
                        {
                            moveCentLeft = false;
                            moveCentDown = true;
                        }

                        // Stop on left side and go up
                        if (cent.xPos <= 15 && goUp)
                        {
                            moveCentLeft = false;
                            moveCentUp = true;
                        }

                        // Move right
                        if (moveCentRight)
                        {
                            // Move Centipede right
                            cent.moveRight(gameTime);
                            centYPos = cent.yPos;
                        }

                        // Stop at right side and go down
                        if ((cent.xPos >= m_graphics.GraphicsDevice.Viewport.Width - 15) && goDown)
                        {
                            moveCentRight = false;
                            moveCentDown = true;
                        }

                        // Stop at right side and go up
                        if ((cent.xPos >= m_graphics.GraphicsDevice.Viewport.Width - 15) && goUp)
                        {
                            moveCentRight = false;
                            moveCentUp = true;
                        }

                        // Move down
                        if (moveCentDown && cent.yPos <= centYPos + 30)
                        {
                            moveCentLeft = false;
                            moveCentRight = false;
                            cent.moveDown(gameTime);
                        }

                        // Move up
                        if (moveCentUp && cent.yPos >= centYPos - 30)
                        {
                            moveCentLeft = false;
                            moveCentRight = false;
                            cent.moveUp(gameTime);
                        }

                        // Go down only so far
                        if (((cent.yPos >= centYPos + 30) || (cent.yPos >= m_graphics.GraphicsDevice.Viewport.Height - 30)) &&
                            !moveCentLeft && !moveCentRight && goDown)
                        {
                            moveCentDown = false;

                            if (hitLeftMush)
                            {
                                moveCentLeft = true;
                                hitLeftMush = false;
                            }
                            else if (hitRightMush)
                            {
                                moveCentRight = true;
                                hitRightMush = false;
                            }
                            else if (cent.xPos >= m_graphics.GraphicsDevice.Viewport.Width - 20)
                            {
                                //moveCentRight = false;
                                moveCentLeft = true;
                            }
                            else if (cent.xPos <= 17)
                            {
                                //moveCentLeft = false;
                                moveCentRight = true;
                            }
                        }

                        // Go up only so far
                        if (((cent.yPos <= centYPos - 30) || (cent.yPos <= 30)) &&
                            !moveCentLeft && !moveCentRight && goUp)
                        {
                            moveCentUp = false;

                            if (hitLeftMush)
                            {
                                moveCentLeft = true;
                                hitLeftMush = false;
                            }
                            else if (hitRightMush)
                            {
                                moveCentRight = true;
                                hitRightMush = false;
                            }
                            else if (cent.xPos >= m_graphics.GraphicsDevice.Viewport.Width - 20)
                            {
                                //moveCentRight = false;
                                moveCentLeft = true;
                            }
                            else if (cent.xPos <= 17)
                            {
                                //moveCentLeft = false;
                                moveCentRight = true;
                            }
                        }

                        // Check mushroom from left side
                        if (((cent.xPos > mushList.X - 25) && (cent.xPos < mushList.X)) &&
                            (cent.yPos <= mushList.Y + 21) && (cent.yPos > mushList.Y - 15)) // 15 used to be 10
                        {
                            if (goDown)
                            {
                                moveCentDown = true;
                            }
                            if (goUp)
                            {
                                moveCentUp = true;
                            }

                            moveCentRight = false;
                            hitLeftMush = true;
                            centYPos = cent.yPos;
                        }

                        // Check mushroom from Right side
                        if (((cent.xPos > mushList.X) && (cent.xPos < mushList.X + 25)) &&
                            (cent.yPos <= mushList.Y + 21) && (cent.yPos > mushList.Y - 15)) // 15 used to be 10
                        {
                            if (goDown)
                            {
                                moveCentDown = true;
                            }
                            if (goUp)
                            {
                                moveCentUp = true;
                            }

                            moveCentLeft = false;
                            hitRightMush = true;
                            centYPos = cent.yPos;
                        }

                        // Check collison on ship
                        if ((m_ShipBox.X >= cent.xPos - 30) && (m_ShipBox.X <= cent.xPos + 30) &&
                            (m_ShipBox.Y >= cent.yPos - 20) && (m_ShipBox.Y <= cent.yPos + 20) && shipAlive)
                        {
                            // Kill ship
                            killShip();
                        }
                    }

                    mushroomList[m] = mushList;
                    //centipedeList[i] = cent;

                }

                centipedeList[i] = cent;
                //mushroomList[m] = mushList;
            }
        }

        private void killShip()
        {
            // Check if player has remaining lives
            if (ship.Lives > 0)
            {
                // Take off life
                ship.takeLives();

                // Move ship rectangle back to starting position
                m_ShipBox.X = m_graphics.GraphicsDevice.Viewport.Width / 2; 
                m_ShipBox.Y = m_graphics.GraphicsDevice.Viewport.Height - 25;

                // Update ship class
                ship.xPos = m_ShipBox.X;
                ship.yPos = m_ShipBox.Y;
            }

            // Game over
            else
            {
                gameLost = true;
                shipAlive = false;
            }             
        }

        public void render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();

            // Draw mushrooms - call a mushroomRender class, pass m_spriteBatch to constructer
            for (int i = 0; i < mushroomList.Count; i++)
            {
                var mush = (Mushroom)mushroomClassList.ElementAt(i);
                if (mush.Hit == 0)
                {
                    spriteBatch.Draw(m_NormMush1Texture, mushroomList[i], Color.White);
                    mushroomClassList[i] = mush;
                }
                if (mush.Hit == 1)
                {
                    spriteBatch.Draw(m_NormMush2Texture, mushroomList[i], Color.White);
                    mushroomClassList[i] = mush;
                }
                if (mush.Hit == 2)
                {
                    spriteBatch.Draw(m_NormMush3Texture, mushroomList[i], Color.White);
                    mushroomClassList[i] = mush;
                }
                if (mush.Hit == 3)
                {
                    spriteBatch.Draw(m_NormMush4Texture, mushroomList[i], Color.White);
                    mushroomClassList[i] = mush;
                }

            }

            // Check if ship is alive 
            if (shipAlive == true)
            {
                // Draw ship
                spriteBatch.Draw(m_ShipTexture, m_ShipBox, Color.White);
            }

            // Draw bullet
            if (bulletList.Count > 0)
            {
                foreach (var value in bulletList)
                {
                    spriteBatch.Draw(m_BulletTexture, value, Color.White);
                }
            }

            // Draw UI
            drawUi(spriteBatch, m_graphics);

            // Draw flee
            if (flea != null)
            {
                fleaRenderer.draw(spriteBatch, flea);
            }

            // Draw spider
            if (spider != null)
            {
                spiderRenderer.draw(spriteBatch, spider);
            }

            // Draw scorpion
            if (scorpion != null)
            {
                scorpionRenderer.draw(spriteBatch, scorpion);
            }

            // Draw centipede
            for (int i  = 0; i < centipedeList.Count; i++)
            {
                var cent = (Objects.Centipede)centipedeList.ElementAt(i);
                if (cent != null)
                {
                    centipedeRenderer.draw(spriteBatch, cent);
                    centipedeList[i] = cent;
                }
            }


            spriteBatch.End();
        }

        private void drawUi(SpriteBatch spriteBatch, GraphicsDeviceManager m_graphics)
        {
            // Draw score
            spriteBatch.DrawString(m_font, score.ToString(), new Vector2(m_graphics.GraphicsDevice.Viewport.Width / 8, 5), Color.Red);

            // Draw high score
            spriteBatch.DrawString(m_font, highScore.ToString(), new Vector2(m_graphics.GraphicsDevice.Viewport.Width / 2 , 5), Color.Red);

            // Draw remaining ship lives
            if (ship.Lives == 3)
            {             
                spriteBatch.Draw(m_ShipTexture, m_ShipBoxLife1, Color.White);
                spriteBatch.Draw(m_ShipTexture, m_ShipBoxLife2, Color.White);
                spriteBatch.Draw(m_ShipTexture, m_ShipBoxLife3, Color.White);
            }
            if (ship.Lives == 2)
            {
                spriteBatch.Draw(m_ShipTexture, m_ShipBoxLife1, Color.White);
                spriteBatch.Draw(m_ShipTexture, m_ShipBoxLife2, Color.White);
            }
            if (ship.Lives == 1)
            {
                spriteBatch.Draw(m_ShipTexture, m_ShipBoxLife1, Color.White);
            }

            // Draw you win
            if (gameWon)
            {                
                spriteBatch.DrawString(m_bigFont, Win_MESSAGE, new Vector2(m_graphics.GraphicsDevice.Viewport.Width / 2, 
                    m_graphics.GraphicsDevice.Viewport.Height / 2), Color.Red);
            }

            // Draw you lost
            if (gameLost)
            {
                spriteBatch.DrawString(m_bigFont, LOST_MESSAGE, new Vector2(m_graphics.GraphicsDevice.Viewport.Width / 2,
                    m_graphics.GraphicsDevice.Viewport.Height / 2), Color.Red);
            }
        }
    }
}
